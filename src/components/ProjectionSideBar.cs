using System.Windows.Forms;

namespace GraphicsComputingApp.src.components
{
    public class ProjectionSideBar
    {
        private readonly Control.ControlCollection controls;
        private readonly int sidebarX;

        public TextBox inputLocalX, inputLocalY, inputLocalZ, inputGlobal;
        public TextBox inputTX, inputTY, inputTZ;
        public RadioButton radioLocal, radioGlobal, radioTranslation, radioOrigem, radioCentro, radioShearing;
        public ComboBox axisComboBox;
        public NumericUpDown degreesInput;
        public TextBox[,] shearingMatrix = new TextBox[4, 4];
        public Button executeButton;

        public event EventHandler ExecuteClicked;

        public ProjectionSideBar(Control.ControlCollection controls, int sidebarX)
        {
            this.controls = controls;
            this.sidebarX = sidebarX;
            BuildSidebar();
        }

        private void BuildSidebar()
        {
            int y = 20;
            y = AddScaleSection(y);
            y = AddTranslationSection(y);
            y = AddRotationSection(y);
            y = AddShearingSection(y);
            AddExecuteButton(y + 20);
        }

        private int AddScaleSection(int y)
        {
            var scaleLabel = new Label
            {
                Text = "Escala",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(sidebarX, y),
                AutoSize = true
            };
            controls.Add(scaleLabel);
            y += 30;

            radioLocal = new RadioButton
            {
                Text = "Local",
                Location = new Point(sidebarX, y),  
                AutoSize = true
            };
            controls.Add(radioLocal);

            int inputX = sidebarX + 70;
            var labelX = new Label { Text = "X", Location = new Point(inputX, y + 3), AutoSize = true };
            inputLocalX = new TextBox { Width = 30, Location = new Point(inputX + 15, y), Text = "1" };
            var labelY = new Label { Text = "Y", Location = new Point(inputX + 50, y + 3), AutoSize = true };
            inputLocalY = new TextBox { Width = 30, Location = new Point(inputX + 65, y), Text = "1" };
            var labelZ = new Label { Text = "Z", Location = new Point(inputX + 100, y + 3), AutoSize = true };
            inputLocalZ = new TextBox { Width = 30, Location = new Point(inputX + 115, y), Text = "1" };

            controls.Add(labelX);
            controls.Add(inputLocalX);
            controls.Add(labelY);
            controls.Add(inputLocalY);
            controls.Add(labelZ);
            controls.Add(inputLocalZ);

            y += 30;

            radioGlobal = new RadioButton
            {
                Text = "Global",
                Location = new Point(sidebarX, y),
                AutoSize = true
            };
            controls.Add(radioGlobal);

            inputGlobal = new TextBox { Width = 60, Location = new Point(sidebarX + 70, y), Text = "1" };
            controls.Add(inputGlobal);

            y += 40;
            return y;
        }

        private int AddTranslationSection(int y)
        {
            var translationLabel = new Label
            {
                Text = "Translação",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(sidebarX, y),
                AutoSize = true
            };
            controls.Add(translationLabel);
            y += 30;

            radioTranslation = new RadioButton
            {
                Text = "Translação",
                Location = new Point(sidebarX, y),
                AutoSize = true
            };
            controls.Add(radioTranslation);

            int inputX = sidebarX + 80;
            var labelTX = new Label { Text = "X", Location = new Point(inputX, y + 3), AutoSize = true };
            inputTX = new TextBox { Width = 30, Location = new Point(inputX + 15, y), Text = "0" };
            var labelTY = new Label { Text = "Y", Location = new Point(inputX + 50, y + 3), AutoSize = true };
            inputTY = new TextBox { Width = 30, Location = new Point(inputX + 65, y), Text = "0" };
            var labelTZ = new Label { Text = "Z", Location = new Point(inputX + 100, y + 3), AutoSize = true };
            inputTZ = new TextBox { Width = 30, Location = new Point(inputX + 115, y), Text = "0" };

            controls.Add(labelTX);
            controls.Add(inputTX);
            controls.Add(labelTY);
            controls.Add(inputTY);
            controls.Add(labelTZ);
            controls.Add(inputTZ);

            return y + 30;
        }

        private int AddRotationSection(int y)
        {
            var rotationLabel = new Label
            {
                Text = "Rotação",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(sidebarX, y),
                AutoSize = true
            };
            controls.Add(rotationLabel);
            y += 30;

            // Radio "Origem"
            radioOrigem = new RadioButton
            {
                Text = "Origem",
                Location = new Point(sidebarX, y),
                AutoSize = true
            };
            controls.Add(radioOrigem);

            var eixoLabel = new Label
            {
                Text = "Eixo",
                Location = new Point(sidebarX + 130, y + 3),
                AutoSize = true
            };
            controls.Add(eixoLabel);

            axisComboBox = new ComboBox
            {
                Location = new Point(sidebarX + 190, y),
                Width = 40,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            axisComboBox.Items.AddRange(new string[] { "X", "Y" });
            axisComboBox.SelectedIndex = 0;
            controls.Add(axisComboBox);

            y += 30;

            radioCentro = new RadioButton
            {
                Text = "Centro do Objeto",
                Location = new Point(sidebarX, y),
                AutoSize = true
            };
            controls.Add(radioCentro);

            var grausLabel = new Label
            {
                Text = "Graus",
                Location = new Point(sidebarX + 130, y + 3),
                AutoSize = true
            };
            controls.Add(grausLabel);

            degreesInput = new NumericUpDown
            {
                Location = new Point(sidebarX + 180, y),
                Width = 50,
                Minimum = -360,
                Maximum = 360,
                DecimalPlaces = 2
            };
            controls.Add(degreesInput);

            y += 40;
            return y;
        }

        private int AddShearingSection(int y)
        {
            var shearingLabel = new Label
            {
                Text = "Shearing",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(sidebarX, y),
                AutoSize = true
            };
            controls.Add(shearingLabel);
            y += 30;

            radioShearing = new RadioButton
            {
                Text = "Shearing",
                Location = new Point(sidebarX, y),
                AutoSize = true
            };
            controls.Add(radioShearing);

            int cellSize = 35;
            int startX = sidebarX + 90;
            int startY = y;

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    var input = new TextBox
                    {
                        Width = 32,
                        Location = new Point(startX + col * cellSize, startY + row * cellSize)
                    };

                    if (row == col)
                    {
                        input.Text = "1";
                        input.ReadOnly = true;
                        input.BackColor = System.Drawing.Color.LightGray;
                    }
                    else if (row == 3 || col == 3)
                    {
                        input.Text = "0";
                        input.ReadOnly = true;
                        input.BackColor = System.Drawing.Color.LightGray;
                    }

                    shearingMatrix[row, col] = input;
                    controls.Add(input);
                }
            }

            shearingMatrix[0, 1].Text = "-0,3";
            shearingMatrix[0, 2].Text = "0,9";
            shearingMatrix[1, 0].Text = "1,3";
            shearingMatrix[1, 2].Text = "-1,2";
            shearingMatrix[2, 0].Text = "2";
            shearingMatrix[2, 1].Text = "2,1";

            y += 4 * cellSize + 10;
            return y;
        }

        private void AddExecuteButton(int y)
        {
            executeButton = new Button
            {
                Text = "EXECUTAR",
                Width = 230,
                Height = 35,
                Location = new Point(sidebarX, y)
            };
            executeButton.Click += (s, e) => ExecuteClicked?.Invoke(this, EventArgs.Empty);
            controls.Add(executeButton);
        }
    }
}