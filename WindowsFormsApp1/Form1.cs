using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using WindowsFormsApp1.control;
using WindowsFormsApp1.PresentationModel;
using WindowsFormsApp1.shape;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        private GroupBox _controlGroup;
        private ComboBox _shapeComboBox;
        private TextBox _textBox, _xBox, _yBox, _heightBox, _widthBox;
        private Button _addButton;
        private DataGridView _dataGridView;
        private Button _page1Button, _page2Button;
        private MenuStrip _menuStrip;
        private Label _textLabel,_xLabel, _yLabel, _heightLabel, _widthLabel;
        private string _originalTitle;
        DoubleBufferedPanel _canvas = new DoubleBufferedPanel();
        private bool _isDrawing = false;
        WindowsFormsApp1.DrawingModel _model;
        PresentationModel.PresentationModel _presentationModel;


        public Form1()
        {
            this.Text = "MyDrawing";
            _model = new WindowsFormsApp1.DrawingModel();
            _presentationModel = new PresentationModel.PresentationModel(_model, _canvas);
            InitializeComponent();
            InitializeControls();
            InitCanvas();
            InitTextBox();
            InitDataBinding();
            toolStripButtonRedo.Enabled = false;
            toolStripButtonUndo.Enabled = false;
            this._model._modelChanged += HandleModelChanged;
            _presentationModel.PMChanged += UpdateUIState;
            _model.EnterPointerState();
            _originalTitle = this.Text;

            // 註冊自動存檔事件
            _model.AutoSaveStarted += Model_AutoSaveStarted;
            _model.AutoSaveCompleted += Model_AutoSaveCompleted;
        }

        private void UpdateUIState()
        {
            Dictionary<ShapeFactory.ShapeType, bool> isChecked = _presentationModel.IsChecked;
            StartShapeButton.Checked = isChecked[ShapeFactory.ShapeType.Start];
            TerminatorShapeButton.Checked = isChecked[ShapeFactory.ShapeType.Terminator];
            DecisionShapeButton.Checked = isChecked[ShapeFactory.ShapeType.Decision];
            ProcessShapeButton.Checked = isChecked[ShapeFactory.ShapeType.Process];
            toolStripButtonLine.Checked = _presentationModel.IsLineChecked;
            toolStripButtonRedo.Enabled = _presentationModel.IsRedoButtonEnabled;
            toolStripButtonUndo.Enabled = _presentationModel.IsUndoButtonEnabled;
        }
        private void Model_AutoSaveStarted(object sender, EventArgs e)
        {
            // 使用 BeginInvoke 確保在 UI 線程上執行
            this.BeginInvoke((MethodInvoker)delegate
            {
                this.Text = $"{_originalTitle} Auto saving...";
            });
        }

        private void Model_AutoSaveCompleted(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                this.Text = _originalTitle;
            });
        }
        private void InitCanvas()
        {
            
            //_canvas.BringToFront();
            _canvas.Location = new Point(163, 52);
            _canvas.Width = 900;
            _canvas.Height = 900;

            this._canvas.BackColor = System.Drawing.Color.LightYellow;
            this._canvas.MouseDown += HandleCanvasPressed;
            this._canvas.MouseUp += HandleCanvasReleased;
            this._canvas.MouseMove += HandleCanvasMoved;
            this._canvas.Paint += HandleCanvasPaint;
            this._canvas.MouseDoubleClick += CanvasMouseDoubleClick;
            this.Controls.Add(_canvas);
           // _model.EnterPointerState();
        }

        private void InitDataBinding()
        {
            // user input
            this._addButton.DataBindings.Add("Enabled", _presentationModel, "IsAddButtonEnabled");
   
            // toolScript
            _textLabel.DataBindings.Add("ForeColor", _presentationModel, "TextLableColor");
            _xLabel.DataBindings.Add("ForeColor", _presentationModel, "XLabelColor");
            _yLabel.DataBindings.Add("ForeColor", _presentationModel, "YLabelColor");
            _heightLabel.DataBindings.Add("ForeColor", _presentationModel, "HLabelColor");
            _widthLabel.DataBindings.Add("ForeColor", _presentationModel, "WLabelColor");
        }
        public void HandleCanvasPressed(object sender, MouseEventArgs e)
        {
           
            _isDrawing = true;
            _presentationModel.PointerPressed(e.X, e.Y);
        }
        public void HandleCanvasReleased(object sender, MouseEventArgs e)
        {
            _isDrawing = false;
            _presentationModel.PointerReleased(e.X, e.Y);
            _presentationModel.UpdateCursor(Cursors.Default);

        }
        public void HandleCanvasMoved(object sender, MouseEventArgs e)
        {
            _presentationModel.PointerMoved(e.X, e.Y);
        }
        public void HandleCanvasPaint(object sender, PaintEventArgs e)
        {
            _presentationModel.Draw(e.Graphics);
        }

        private void InitializeControls()
        {

            // Initialize MenuStrip
            _menuStrip = new MenuStrip();
            ToolStripMenuItem helpMenu = new ToolStripMenuItem("說明");
            ToolStripMenuItem aboutItem = new ToolStripMenuItem("關於");
            helpMenu.DropDownItems.Add(aboutItem);
            _menuStrip.Items.Add(helpMenu);
            _menuStrip.Dock = DockStyle.Top;  // 設定 MenuStrip 固定在頂部
            this.Controls.Add(_menuStrip);

            // Initialize Page Buttons
            _page1Button = new Button { BackColor = Color.White, Location = new Point(10, 180), Size = new Size(150, 120) };

            _page2Button = new Button { BackColor = Color.White, Location = new Point(10, 500), Size = new Size(150, 120) };
            this.Controls.Add(_page1Button);
            this.Controls.Add(_page2Button);


            // Initialize Control Group
            _controlGroup = new GroupBox
            {
                Text = "資料顯示",
                Dock = DockStyle.Right,
                Size = new Size(380, 1000)
            };

            // Shape ComboBox
            _shapeComboBox = new ComboBox
            {
                Location = new Point(90, 30),
                Size = new Size(85, 20)
            };
            _shapeComboBox.Items.AddRange(new object[] { "Start", "Terminator", "Process", "Decision" });

            // TextBoxes
            // 初始化 Label 和 TextBox
            var textBoxGroup = CreateTextBox("Text", new Point(190, 10));
            _textBox = textBoxGroup.TextBox;
            _textLabel = textBoxGroup.Label; // 獲取 Label 以便後續綁定

            // 初始化其他組合
            var xBoxGroup = CreateTextBox("X", new Point(230, 10));
            _xBox = xBoxGroup.TextBox;
            _xLabel = xBoxGroup.Label;

            var yBoxGroup = CreateTextBox("Y", new Point(270, 10));
            _yBox = yBoxGroup.TextBox;
            _yLabel = yBoxGroup.Label;
           
            var heightBoxGroup = CreateTextBox("H", new Point(310, 10));
            _heightBox = heightBoxGroup.TextBox;
            _heightLabel = heightBoxGroup.Label;

            var widthBoxGroup = CreateTextBox("W", new Point(345, 10));
            _widthBox = widthBoxGroup.TextBox;
            _widthLabel = widthBoxGroup.Label;

            // Add Button
            _addButton = new Button
            {
                Text = "Add",
                Location = new Point(10, 15),
                Size = new Size(70, 40)
            };
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
            {

                BackColor = Color.Gray, // 背景設為灰色
                
                Location = new Point(0, 50),
                FlowDirection = FlowDirection.TopDown, // 垂直排列按鈕
                Size = new Size(160, 1000),
                Padding = new Padding(0, 20, 0, 0) // 頂部留 20px 空間

            };
            flowLayoutPanel.Controls.Add(_page1Button);
            flowLayoutPanel.Controls.Add(_page2Button);
            // 將 FlowLayoutPanel 加入表單
            this.Controls.Add(flowLayoutPanel);


            _addButton.Click += AddButton_Click;

            // DataGridView
            _dataGridView = new DataGridView
            {
                Location = new Point(10, 100),
                Size = new Size(500, 500),
                AllowUserToAddRows = false,
                ReadOnly = true,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false

            };
            SetupDataGridView();

            // Add controls to group
            _controlGroup.Controls.AddRange(new Control[] {
            _shapeComboBox, _textBox, _xBox, _yBox, _heightBox, _widthBox,
            _addButton, _dataGridView
        });

            this.Controls.Add(_controlGroup);



        }
        // 新增 textbox 
        private (Label Label, TextBox TextBox) CreateTextBox(string labelText, Point location)
        {
            Label label = new Label
            {
                Text = labelText,
                Location = location,
                Size = new Size(30, 20)
            };
            TextBox textBox = new TextBox
            {
                Location = new Point(location.X - 5, location.Y + 20),
                Size = new Size(30, 20)
            };
            // 將控制項新增到容器
            _controlGroup.Controls.Add(label);
            _controlGroup.Controls.Add(textBox);
            return (label, textBox);
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }


        public void HandleToolStripButtonClick(ToolStripButton button, ShapeFactory.ShapeType shapeName)
        {
            _presentationModel.SetCheckedButton(shapeName);
            _presentationModel.UpdateCursor(Cursors.Cross);
            
        }
       
        private void StartShapeButton_Click(object sender, EventArgs e)
        {
            HandleToolStripButtonClick(StartShapeButton, ShapeFactory.ShapeType.Start);
        }

        private void TerminatorShapeButton_Click(object sender, EventArgs e)
        {
            HandleToolStripButtonClick(TerminatorShapeButton, ShapeFactory.ShapeType.Terminator);
        }

        private void DecisionShapeButton_Click(object sender, EventArgs e)
        {
            HandleToolStripButtonClick(DecisionShapeButton, ShapeFactory.ShapeType.Decision);
        }

        private void ProcessShapeButton_Click(object sender, EventArgs e)
        {
            HandleToolStripButtonClick(ProcessShapeButton, ShapeFactory.ShapeType.Process);
        }

        private void toolStripButtonLine_Click(object sender, EventArgs e)
        {
             _presentationModel.SetCheckedButton(ShapeFactory.ShapeType.Process, true); // 使用特殊參數表示這是 Line
            _presentationModel.UpdateCursor(Cursors.Cross);
        }

        private void toolStripButtonUndo_Click(object sender, EventArgs e)
        {
            this._presentationModel.Undo();
        }

        private void toolStripButtonRedo_Click(object sender, EventArgs e)
        {
            this._presentationModel.Redo();
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Flow Chart Files (*.flow)|*.flow";
            saveFileDialog.DefaultExt = "flow";
            saveFileDialog.AddExtension = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                saveButton.Enabled = false;
                try
                {
                    await _model.SaveToFileAsync(saveFileDialog.FileName);
                    Console.WriteLine("good");
                    
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"儲存失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    saveButton.Enabled = true;
                }
            }
        }
       

        private void loadButton_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Flow Chart Files (*.flow)|*.flow";
                openFileDialog.DefaultExt = "flow";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    bool success = _model.LoadFromFile(openFileDialog.FileName);
                    this.Cursor = Cursors.Default;

                }
            }
        }

        private void NormalModeButton_Click(object sender, EventArgs e)
        {
            _presentationModel.ClearAllCheckedButtons();
            _presentationModel.SetToPointState();
            _presentationModel.UpdateCursor(Cursors.Default);
        }

        // 建立 DataGridView
        private void SetupDataGridView()
        {
            _dataGridView.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "Delete",
                Text = "Delete",
                UseColumnTextForButtonValue = true,
                Width = 50
            });
            _dataGridView.Columns.Add("Id", "ID");
            _dataGridView.Columns["Id"].Width = 40;
            _dataGridView.Columns.Add("ShapeType", "形狀");
            _dataGridView.Columns["ShapeType"].Width = 70;
            _dataGridView.Columns.Add("Text", "文字");
            _dataGridView.Columns["Text"].Width = 60;
            _dataGridView.Columns.Add("X", "X");
            _dataGridView.Columns["X"].Width = 40;
            _dataGridView.Columns.Add("Y", "Y");
            _dataGridView.Columns["Y"].Width = 40;
            _dataGridView.Columns.Add("Height", "H");
            _dataGridView.Columns["Height"].Width = 40;
            _dataGridView.Columns.Add("Width", "W");
            _dataGridView.Columns["Width"].Width = 27;

            _dataGridView.CellClick += DataGridView_CellClick;
        }
        private void InitTextBox()
        {
            // 為所有的 TextBox 註冊同一個事件處理方法
            // 註冊 ComboBox 的事件
            this._shapeComboBox.SelectedIndexChanged += HandleInputChanged;
            this._textBox.TextChanged += HandleInputChanged;
            this._xBox.TextChanged += HandleInputChanged;
            this._yBox.TextChanged += HandleInputChanged;
            this._heightBox.TextChanged += HandleInputChanged;
            this._widthBox.TextChanged += HandleInputChanged;
        }
        private void CanvasMouseDoubleClick(object sender, MouseEventArgs e)
        {
            
            if (_presentationModel.IsContentDoubleClick(e.X, e.Y))
            {
                EditText form = new EditText(_model._selectShape.Text);
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _presentationModel.EditSelectedContent(form.TextBoxContent);
                }
            }
        }
        private void HandleInputChanged(object sender, EventArgs e)
        {
            // 當任何一個 TextBox 內容改變時，都會呼叫 AddShapeDataChanged
            _presentationModel.AddShapeDataChanged(
                this._shapeComboBox.Text,
                this._textBox.Text,  // 假設 name 和 shape 使用相同的值
                this._xBox.Text,
                this._yBox.Text,
                this._heightBox.Text,
                this._widthBox.Text
            );
        }
        private void AddButton_Click(object sender, EventArgs e)
        {
            ShapeFactory.ShapeType shapeType = (ShapeFactory.ShapeType)Enum.Parse(typeof(ShapeFactory.ShapeType), _shapeComboBox.Text);
            int x = int.Parse(_xBox.Text);
            int y = int.Parse(_yBox.Text);
            int width = int.Parse(_heightBox.Text);
            int height = int.Parse(_widthBox.Text);
            _model.AddShape(shapeType, _textBox.Text,x, y, height, width);
            UpdateUIState();
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 當刪除按鈕欄被點擊時
            if (e.ColumnIndex == 0)
                _presentationModel.RemoveShapeAt(e.RowIndex);
        }


        public void HandleModelChanged()
        {
            this._canvas.Invalidate(true);
            if (!_isDrawing)
                UpdateShapeList();
        }
        public void UpdateShapeList()
        {
            IList<Shape> shapes = _model.GetShapes();
            int diff = Math.Abs(_dataGridView.Rows.Count - shapes.Count);
            if (_dataGridView.Rows.Count < shapes.Count)
                for (int i = 0; i < diff; i++)
                    _dataGridView.Rows.Add();
            else if (_dataGridView.Rows.Count > shapes.Count)
                for (int i = 0; i < diff; i++)
                    _dataGridView.Rows.RemoveAt(0);
            for (int i = 0; i < shapes.Count; i++)
            {
                Shape shape = shapes[i];
                object[] value;
                if (shape is Line line)
                {
                    value = new object[] { "刪除", i + 1, "Line", "", line.EndX,line.EndY, Math.Abs(line.EndX - line.EndY),13};
                }
                else
                {
                    value = new object[] { "刪除", i + 1, shape.GetShapeType(), shape.Text, shape.X, shape.Y, shape.Height, shape.Width };
                }
                for (int j = 0; j < value.Length; j++)
                   
                    if (!object.Equals(_dataGridView.Rows[i].Cells[j].Value, value[j]))
                        _dataGridView.Rows[i].Cells[j].Value = value[j];
            }

        }
    }
}
