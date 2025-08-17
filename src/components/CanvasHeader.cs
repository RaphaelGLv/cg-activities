namespace GraphicsComputingApp.components;

    public class CanvasHeader
    {
        private const int ItemsGap = 10;
        private const int ItemsHorizontalPadding = 4;
        private const int CharWidth = 6;

        private readonly Control.ControlCollection controls;
        private int _containerWidth = ItemsGap;
        private ComboBox _functionsComboBox;
        
        private readonly Dictionary<string, EventHandler> _actions = new();

        public CanvasHeader(Control.ControlCollection controls)
        {
            this.controls = controls;
        }
        
        public void CreateButton(string text, EventHandler onClick)
        {
            var button = new Button();

            button.Text = text;

            button.Click += onClick;
            button.Width = _CalculateButtonWidth(text);
            button.Location = new Point(_containerWidth + ItemsGap, 10);

            controls.Add(button);
            _containerWidth += button.Width + ItemsGap;
        }
        public void CreateComboBox(List<DrawingFunctionOption> options)
        {
            const int downArrowIconWidth = 8;
            const string comboBoxTitle = "Selecione uma função de desenho...";
            
            _functionsComboBox = new ComboBox
            {
                Name = "functionsComboBox",
                Location = new Point(_containerWidth, 10),
                Width = _CalculateButtonWidth(comboBoxTitle) + downArrowIconWidth,
                DropDownStyle = ComboBoxStyle.DropDownList

            };

            _functionsComboBox.Items.Add(comboBoxTitle);
            _functionsComboBox.SelectedIndex = 0;

            _functionsComboBox.SelectedIndexChanged += _OnComboBoxFunctionSelected;

            controls.Add(_functionsComboBox);
            _containerWidth += _functionsComboBox.Width;

            foreach (var option in options)
            {
                _functionsComboBox.Items.Add(option.Title);
                _actions[option.Title] = option.OnClick;
            }
        }

        private void _OnComboBoxFunctionSelected(object sender, EventArgs e)
        {
            var selectedFunction = _functionsComboBox.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedFunction) && _actions.TryGetValue(selectedFunction, out var actionToExecute))
            {
                actionToExecute.Invoke(_functionsComboBox, e);
            }
        }

        private int _CalculateButtonWidth(string buttonText)
        {
            return (buttonText.Length * CharWidth) + ItemsHorizontalPadding;
        }
    }