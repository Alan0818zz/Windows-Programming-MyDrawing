using System.Collections.Generic;
using WindowsFormsApp1.command;
using WindowsFormsApp1.shape;
using WindowsFormsApp1.state;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace WindowsFormsApp1
{
    public class DrawingModel
    {
        public delegate void ModelChangedEventHandler();
        public event ModelChangedEventHandler _modelChanged;
        public PresentationModel.PresentationModel _presentationModel;
        public CommandManager commandManager = new CommandManager();
        // State pattern
        public  IState currentState;
        public IState pointerState;
        public IState drawingState;
        //public IState linestate;
        //沒實作line 所以用圖形判斷未完成的圖形繪製
        ShapeFactory.ShapeType inProgressShapeType;
       
        public Shape _selectShape = null;
        public Shape hoverShape = null;
        private ShapeFactory factory = new ShapeFactory();
        // 連線s
        readonly private List<Line> lines = new List<Line>();
        public Line notCompleteLine = null;
        private Shapes _shapes = new Shapes();
        private bool _isModified = false;
        private System.Timers.Timer _autoSaveTimer;
        private const string BACKUP_FOLDER = "drawing_backup";
        private const int MAX_BACKUP_FILES = 5;
        public DrawingModel()
        {
            pointerState = new PointerState(this);
            // 建立drawingState物件，令DrawingState知道PointerState
            drawingState = new DrawingState(this);
            EnterPointerState();
            _autoSaveTimer = new System.Timers.Timer(30000); // 30秒
            _autoSaveTimer.Elapsed += AutoSaveTimer_Elapsed;
            _autoSaveTimer.Start();
        }
        // 在所有會修改 Model 的方法中（如 AddShape, RemoveShape 等）加入：
        private void SetModified()
        {
            _isModified = true;
        }
        private async void AutoSaveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_isModified)
                return;

            _autoSaveTimer.Stop(); // 暫停計時器避免重複觸發
            try
            {
                // 通知 Form 開始自動存檔
                AutoSaveStarted?.Invoke(this, EventArgs.Empty);

                // 確保備份資料夾存在
                string backupPath = Path.Combine(
     Directory.GetCurrentDirectory(),
     BACKUP_FOLDER
 );
                Directory.CreateDirectory(backupPath);

                // 產生檔名
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_bak.mydrawing";
                string fullPath = Path.Combine(backupPath, fileName);

                // 執行存檔
                await SaveToFileAsync(fullPath);

                // 清理舊檔案
                CleanupOldBackups(backupPath);
                _isModified = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Auto save failed: {ex.Message}");
            }
            finally
            {
                // 通知 Form 自動存檔結束
                AutoSaveCompleted?.Invoke(this, EventArgs.Empty);
                _autoSaveTimer.Start(); // 重新啟動計時器
            }
        }
        private void CleanupOldBackups(string backupPath)
        {
            var files = Directory.GetFiles(backupPath, "*_bak.mydrawing")
                .OrderByDescending(f => f)
                .Skip(MAX_BACKUP_FILES);

            foreach (var file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete old backup: {ex.Message}");
                }
            }
        }// 事件定義
        public event EventHandler AutoSaveStarted;
        public event EventHandler AutoSaveCompleted;

       
        public void SetPresentationModel(PresentationModel.PresentationModel presentationModel)
        {
            _presentationModel = presentationModel;

        }
        public void AddShape(ShapeFactory.ShapeType shapeType, string text, int x, int y, int height, int width)
        {
           
            Shape shape = factory.CreateShape(shapeType);
            shape.Text = text;
            shape.X = x;
            shape.Y = y;
            shape.Height = height;
            shape.Width = width;
            this.commandManager.Execute(new AddCommand(this, shape));
           
            NotifyModelChanged(); // 通知視圖模型已更改
            EnterPointerState();
           
        }
        public void AddShape(Shape shape)
        {
            _shapes.AddShapes(shape);
            SetModified();
            NotifyModelChanged(); // 通知視圖模型已更改
        }
        public void EditSelectedContent(string content)
        {
            this.commandManager.Execute(new TextChangeCommand(this._selectShape, content));
            NotifyModelChanged();
        }
        public void RemoveShape(int index)
        {
            _shapes.RemoveShapeByIndex(index);
            _selectShape = null;
            NotifyModelChanged();
        }
       
        public IList<Shape> GetShapes()
        {
            return _shapes.GetShapes();
        }
        public void AddLine(Line line)
        {
            if (line == null)
                return;
            this.lines.Add(line);
            this._shapes.AddShapes(line);
            NotifyModelChanged();
        }

      
        public void InsertShape(int index, Shape shape)
        {
            this._shapes.GetShapes().Insert(index, shape);
         
        }
        public void Draw(IGraphics graphics)
        {
            graphics.ClearAll();
            foreach (Shape shape in _shapes.GetShapes())
            {
                if(shape is Line line)
                {
                    line.Draw(graphics);
                }
                else
                {
                    shape.Draw(graphics);
                }
               
            }
            _selectShape?.DrawDrawBoundingBox(graphics);
            currentState.OnPaint(graphics);
            notCompleteLine?.Draw(graphics);
            hoverShape?.DrawConnectionPoints(graphics);
            NotifyModelChanged();
        }
        public void Execute(ICommand command)
        {
            commandManager.Execute(command);
            NotifyModelChanged();
        }
        public void Undo()
        {
            this.commandManager.Undo();
            NotifyModelChanged();
        }
        public void Redo()
        {
            this.commandManager.Redo();
            NotifyModelChanged();
        }
        public void PointerPressed(double x, double y)
        {
            currentState.MouseDown(this, x,y , this.inProgressShapeType);
            
        }
        public void PointerMoved(double x, double y)
        {
            currentState.MouseMove(this, x,y);
        }
        public void ChooseShapeType(ShapeFactory.ShapeType shapeType)
        {
           this.inProgressShapeType = shapeType;
        }
        
        public void PointerReleased(double x, double y)
        {
            currentState.MouseUp(this, x , y);
            
        }

        // 進入DrawingState
        public void EnterDrawingState()
        {
            currentState = drawingState;
            drawingState.Initialize();
            this._selectShape = null;
            NotifyModelChanged();
        }
        // 進入pointerstae
        public void EnterPointerState()
        {
            currentState = pointerState;
            pointerState.Initialize();
        }
        // 進入linestate
        public void EnterLineState(DrawingLineState lineState)
        {
            currentState = lineState;
            lineState.Initialize();
        }
        public void NotifyModelChanged()
        {
            _modelChanged?.Invoke();
        }

        public async Task SaveToFileAsync(string fileName)
        {
            try
            {
                // 先收集要儲存的資料
                var shapesToSave = _shapes.GetShapes().Where(s => !(s is Line)).ToList();
                var saveData = new SaveData
                {
                    Shapes = new List<ShapeData>(),
                    Lines = new List<LineData>()
                }; // 儲存一般圖形
                foreach (var shape in shapesToSave)
                {
                    saveData.Shapes.Add(new ShapeData
                    {
                        Type = GetShapeTypeFromShape(shape),
                        Text = shape.Text,
                        X = shape.X,
                        Y = shape.Y,
                        Width = shape.Width,
                        Height = shape.Height,
                        TextOffsetX = shape.TextOffsetX,
                        TextOffsetY = shape.TextOffsetY
                    });
                }// 儲存線條
                foreach (var line in lines)
                {
                    saveData.Lines.Add(new LineData
                    {
                        StartShapeIndex = shapesToSave.IndexOf(line.StartShape),
                        EndShapeIndex = shapesToSave.IndexOf(line.EndShape),
                        StartX = line.StartX,
                        StartY = line.StartY,
                        EndX = line.EndX,
                        EndY = line.EndY,
                        StartConnectionLocation = line.StartConnectionLocation,
                        EndConnectionLocation = line.EndConnectionLocation
                    });
                } // 使用 Task.Run 進行實際的檔案寫入
                await Task.Run(() =>
                {
                    Thread.Sleep(3000); // 模擬延遲
                    using (FileStream fs = new FileStream(fileName, FileMode.Create))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(fs, saveData);
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"儲存失敗: {ex.Message}");
            }
        }
        public bool LoadFromFile(string fileName)
        {
            try
            {
                SaveData saveData;
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    saveData = (SaveData)formatter.Deserialize(fs);
                }

                // 清除現有資料
                _shapes = new Shapes();
                lines.Clear();
                _selectShape = null;
                hoverShape = null;
                notCompleteLine = null;
                // 載入一般圖形
                Dictionary<int, Shape> indexToShape = new Dictionary<int, Shape>();
                for (int i = 0; i < saveData.Shapes.Count; i++)
                {
                    var shapeData = saveData.Shapes[i];
                    Shape shape = factory.CreateShape(shapeData.Type);
                    shape.Text = shapeData.Text;
                    shape.X = shapeData.X;
                    shape.Y = shapeData.Y;
                    shape.Width = shapeData.Width;
                    shape.Height = shapeData.Height;
                    shape.TextOffsetX = shapeData.TextOffsetX;
                    shape.TextOffsetY = shapeData.TextOffsetY;

                    _shapes.AddShapes(shape);
                    indexToShape[i] = shape;
                }// 載入線條
                foreach (var lineData in saveData.Lines)
                {
                    if (indexToShape.ContainsKey(lineData.StartShapeIndex) &&
                        indexToShape.ContainsKey(lineData.EndShapeIndex))
                    {
                        Line line = new Line();
                        line.StartShape = indexToShape[lineData.StartShapeIndex];
                        line.EndShape = indexToShape[lineData.EndShapeIndex];
                        line.StartX = lineData.StartX;
                        line.StartY = lineData.StartY;
                        line.EndX = lineData.EndX;
                        line.EndY = lineData.EndY;
                        line.StartConnectionLocation = lineData.StartConnectionLocation;
                        line.EndConnectionLocation = lineData.EndConnectionLocation;

                        lines.Add(line);
                        _shapes.AddShapes(line);
                    }
                }

                NotifyModelChanged();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Load error: {ex.Message}");
                return false;
            }
        }

        private ShapeFactory.ShapeType GetShapeTypeFromShape(Shape shape)
        {
            if (shape is Start) return ShapeFactory.ShapeType.Start;
            if (shape is Terminator) return ShapeFactory.ShapeType.Terminator;
            if (shape is Process) return ShapeFactory.ShapeType.Process;
            if (shape is Decision) return ShapeFactory.ShapeType.Decision;
            throw new ArgumentException("Unknown shape type");
        }



    }
}
