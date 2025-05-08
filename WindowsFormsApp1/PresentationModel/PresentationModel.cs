using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WindowsFormsApp1.command;
using WindowsFormsApp1.state;


namespace WindowsFormsApp1.PresentationModel
{
    public class PresentationModel : INotifyPropertyChanged
    {
        DrawingModel _model;
        
        Dictionary<ShapeFactory.ShapeType, bool> _isChecked = new Dictionary<ShapeFactory.ShapeType, bool>(); // 儲存各形狀是否被選中的字典
                                                                                                              // 在現有的 Dictionary 宣告下方加入 Line 的狀態
        private bool _isLineChecked = false;
        public event PropertyChangedEventHandler PropertyChanged;
        
        public event PresentationModelChangedEventHandler PMChanged;
        public delegate void PresentationModelChangedEventHandler();
        private readonly Control _canvas;

      
        readonly private DrawingLineState drawLineState;
        public PointerState pointerState;
        public DrawingState drawingState;
        // btnAdd
        bool isAddButtonEnabled = false;
        // label color
        Color _textLabelColor;
        Color _xLabelColor; 
        Color _yLabelColor;
        Color _wLabelColor; 
        Color _hLabelColor; 

        public PresentationModel(DrawingModel model, Control canvas)
        {
            
            this._model = model;
            this._canvas = canvas;
            this.drawLineState = new DrawingLineState(this._model, this);
            this.pointerState = new PointerState(this._model);
            this.drawingState = new DrawingState(this._model);
            this._model.SetPresentationModel(this);

            _isChecked.Add(ShapeFactory.ShapeType.Start, false);
            _isChecked.Add(ShapeFactory.ShapeType.Terminator, false);
            _isChecked.Add(ShapeFactory.ShapeType.Process, false);
            _isChecked.Add(ShapeFactory.ShapeType.Decision, false);
            _isLineChecked = false; // 初始化 Line 狀態

            // 初始 label color
            _textLabelColor = Color.Red;
            _xLabelColor = Color.Red;
            _yLabelColor = Color.Red;
            _wLabelColor = Color.Red;
            _hLabelColor = Color.Red;
        }
        public bool IsContentDoubleClick(double x, double y)
        {
            if (_model._selectShape == null)
                return false;
            return _model._selectShape.IsPointInTextHandle(x, y);
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Draw(Graphics graphics)
        {
            _model.Draw(new WindowsFormsGraphicsAdaptor(graphics));
        }

        public void SetCheckedButton(ShapeFactory.ShapeType shapeName, bool isLine = false)
        {
            ClearAllCheckedButtons();
            if (isLine)
            {
                _isLineChecked = true;
                _model.EnterLineState(drawLineState); // 進入 Line 
            }
            else
            {
                _isChecked[shapeName] = true;
                _model.EnterDrawingState();
            }
            NotifyPMChanged();
        }
        // 返回 _isChecked
        public Dictionary<ShapeFactory.ShapeType, bool> IsChecked
        {
            get
            {
                return _isChecked;
            }
        }
        public void EditSelectedContent(string content)
        {
            if (this._model._selectShape == null)
                return;
            _model.EditSelectedContent(content);
            //pointerState._isDraggingText = false;
            NotifyPMChanged();
        }
        // 重製所有 checked 為 false
        public void ClearAllCheckedButtons()
        {
            _isChecked[ShapeFactory.ShapeType.Start] = false;
            _isChecked[ShapeFactory.ShapeType.Terminator] = false;
            _isChecked[ShapeFactory.ShapeType.Process] = false;
            _isChecked[ShapeFactory.ShapeType.Decision] = false;
            _isLineChecked = false;
            _model.EnterPointerState();
            NotifyPMChanged();
        }

        public void SetToPointState()
        {
          _model.EnterPointerState();
          NotifyPMChanged();
        }
        public bool IsAnyShapeChecked()
        {
            if (_isLineChecked) return true;
            foreach (var item in _isChecked)
            {
                if (item.Value)
                {
                    return true;
                }
            }
            return false;
        }
       
        public void Undo()
        {
            _model.Undo();
            NotifyPMChanged();
        }
        public void Redo()
        {
            _model.Redo();
            NotifyPMChanged();
        }
        public void RemoveShapeAt(int index)
        {
            this._model.Execute(new DeleteCommand(this._model, index));
            NotifyPMChanged();
        }
        // 放開 鼠標
        public void PointerReleased(double x , double y)
        {
            _model.PointerReleased(x,y);
            if (IsAnyShapeChecked())
                ClearAllCheckedButtons();
        }
        // 移動鼠標
        public void PointerMoved(double x, double y)
        {
            _model.PointerMoved(x, y);
        }
        // 鼠標按下 開始畫圖
        public void PointerPressed(double x , double y)
        {
            foreach (var item in _isChecked)
            {
                if (item.Value)
                {
                    _model.ChooseShapeType(item.Key);
                    _model.PointerPressed(x, y);
                    return;
                }
            }
            _model.PointerPressed(x,y);
        }
        
        // 更改數標樣式
        public void UpdateCursor(Cursor cursor)
        {
            _canvas.Cursor = cursor;
        }
        public virtual void NotifyPMChanged()
        {
            PMChanged?.Invoke();
        }
        public void AddShapeDataChanged(string shape, string content, string x, string y, string height, string width)
        {
            bool shapeCheck = string.IsNullOrEmpty(shape);
            bool contentCheck = string.IsNullOrEmpty(content);
            bool xCheck = int.TryParse(x, out _);
            bool yCheck = int.TryParse(y, out _);
            bool widthCheck = int.TryParse(width, out _);
            bool heightCheck = int.TryParse(height, out _);

            // 通知屬性已更改
            LabelShapeContentChange(contentCheck);
            LabelShapeXChange(xCheck);
            LabelShapeYChange(yCheck);
            LabelShapeWChange(widthCheck);
            LabelShapeHChange(heightCheck);
            isAddButtonEnabled = !shapeCheck && !contentCheck && xCheck && yCheck && widthCheck && heightCheck;
            OnPropertyChanged("IsAddButtonEnabled");
            
        }
        public void LabelShapeContentChange(bool contentCheck)
        {
            if (contentCheck)
                _textLabelColor = Color.Red;
            else
                _textLabelColor = Color.Black;
            OnPropertyChanged("TextLableColor");

        }
        public void LabelShapeXChange(bool xCheck)
        {
            if (!xCheck)
                _xLabelColor = Color.Red;
            else
                _xLabelColor = Color.Black;
            OnPropertyChanged("XLabelColor");
           
        }
        public void LabelShapeYChange(bool yCheck)
        {
            if (!yCheck)
                _yLabelColor = Color.Red;
            else
                _yLabelColor = Color.Black;
            OnPropertyChanged("YLabelColor");
            
        }
        public void LabelShapeWChange(bool widthCheck)
        {
            if (!widthCheck)
                _wLabelColor = Color.Red;
            else
                _wLabelColor = Color.Black;
            OnPropertyChanged("WLabelColor");
            
        }
        public void LabelShapeHChange(bool heightCheck)
        {
            if (!heightCheck)
                _hLabelColor = Color.Red;
            else
                _hLabelColor = Color.Black;
            OnPropertyChanged("HLabelColor");
           
        }
        // 新增 Line 的 checked 屬性
        public bool IsLineChecked
        {
            get => _isLineChecked;
        }

        public bool IsAddButtonEnabled
        {
            get
            {
                return isAddButtonEnabled;
            }
        }

        // 取得新增形狀標籤的顏色

        public Color TextLableColor
        {
            get
            {
                return _textLabelColor;
            }
        }
        public Color XLabelColor
        {
            get
            {
                return _xLabelColor;
            }
        }

        // 取得新增形狀標籤的顏色
        public Color YLabelColor
        {
            get
            {
                return _yLabelColor;
            }
        }

        // 取得新增形狀標籤的顏色
        public Color WLabelColor
        {
            get
            {
                return _wLabelColor;
            }
        }

        // 取得新增形狀標籤的顏色
        public Color HLabelColor
        {
            get
            {
                return _hLabelColor;
            }
        }
        // redo undo 按鈕相關
        public bool IsRedoButtonEnabled
        {
            get { return _model.commandManager.IsRedoEnabled; }
        }
        public bool IsUndoButtonEnabled
        {
            get { return _model.commandManager.IsUndoEnabled; }
        }
    }
}
