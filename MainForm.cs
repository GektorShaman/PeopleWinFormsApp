using Microsoft.EntityFrameworkCore;
using PeopleApp.Models;

namespace PeopleApp
{
    public partial class MainForm : Form
    {
        private readonly AppDbContext _context;

        private DataGridView dgvPeople;
        private TextBox txtName;
        private NumericUpDown numAge;
        private DateTimePicker dtpBirthdate;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;
        private ToolTip toolTip;

        public MainForm(DbContextOptions<AppDbContext> options)
        {
            _context = new AppDbContext(options);
            InitComponent();
            LoadPeople();
        }

        private void InitComponent()
        {
            Text = "People Manager";
            Width = 800;
            Height = 600;

            toolTip = new ToolTip();
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;

            dgvPeople = new DataGridView { Left = 10, Top = 10, Width = 760, Height = 300, ReadOnly = true, AutoGenerateColumns = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            dgvPeople.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id", DataPropertyName = "Id", Visible = false });
            dgvPeople.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Name", DataPropertyName = "Name", Width = 300 });
            dgvPeople.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Age", DataPropertyName = "Age", Width = 80 });
            dgvPeople.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Birthdate", DataPropertyName = "Birthdate", Width = 150 });
            dgvPeople.CellDoubleClick += DgvPeople_CellDoubleClick;

            var lblName = new Label { Text = "Name:", Left = 10, Top = 330, Width = 50 };
            txtName = new TextBox { Left = 70, Top = 325, Width = 300 };

            var lblAge = new Label { Text = "Age:", Left = 390, Top = 330, Width = 40 };
            numAge = new NumericUpDown { Left = 430, Top = 325, Width = 60, Minimum = 0, Maximum = 150 };

            var lblBirth = new Label { Text = "Birthdate:", Left = 510, Top = 330, Width = 70 };
            dtpBirthdate = new DateTimePicker { Left = 585, Top = 325, Width = 185, Format = DateTimePickerFormat.Short };

            btnAdd = new Button { Text = "Добавить", Left = 10, Top = 370, Width = 100 };
            btnUpdate = new Button
            {
                Text = " Обновить",
                Left = 120,
                Top = 370,
                Width = 100,
                Image = CreateInfoIcon(),
                ImageAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.TextBeforeImage
            };
            btnDelete = new Button { Text = "Удалить", Left = 230, Top = 370, Width = 100 };
            btnClear = new Button { Text = "Очистить", Left = 340, Top = 370, Width = 100 };

            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            btnClear.Click += BtnClear_Click;

            toolTip.SetToolTip(btnUpdate, "Для выбора записи необходимо двойное нажатие по записи");

            Controls.Add(dgvPeople);
            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(lblAge);
            Controls.Add(numAge);
            Controls.Add(lblBirth);
            Controls.Add(dtpBirthdate);
            Controls.Add(btnAdd);
            Controls.Add(btnUpdate);
            Controls.Add(btnDelete);
            Controls.Add(btnClear);
        }

        private Bitmap CreateInfoIcon()
        {
            var bitmap = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(bitmap))
            using (var font = new Font("Arial", 10, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.Blue))
            {
                g.Clear(Color.Transparent);
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.DrawString("i", font, brush, 4, 1);
            }
            return bitmap;
        }

        private void LoadPeople()
        {
            try
            {
                var people = _context.People.OrderBy(p => p.Name).ToList();
                dgvPeople.DataSource = people;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            AddPerson();
        }

        private void AddPerson()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Заполните имя", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtName.Text, @"^[\p{L}\s\-]+$"))
            {
                MessageBox.Show("Имя может содержать только буквы, пробелы и дефисы", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if ((int)numAge.Value == 0)
            {
                MessageBox.Show("Выставьте возраст", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtpBirthdate.Value > DateTime.Today)
            {
                MessageBox.Show("Дата рождения не может быть в будущем", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var person = new Person
            {
                Name = txtName.Text,
                Age = (int)numAge.Value,
                Birthdate = dtpBirthdate.Value.Date
            };

            try
            {
                _context.People.Add(person);
                _context.SaveChanges();
                LoadPeople();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            UpdatePerson();
        }

        private void UpdatePerson()
        {
            var idObj = dgvPeople.CurrentRow.Cells[0].Value;
            if (idObj == null) return;

            Guid id = (Guid)idObj;

            var person = _context.People.Find(id);
            if (person == null)
            {
                MessageBox.Show("Запись не найдена", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Заполните имя", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtName.Text, @"^[\p{L}\s\-]+$"))
            {
                MessageBox.Show("Имя может содержать только буквы, пробелы и дефисы", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if ((int)numAge.Value == 0)
            {
                MessageBox.Show("Выставьте возраст", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtpBirthdate.Value > DateTime.Today)
            {
                MessageBox.Show("Дата рождения не может быть в будущем", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            person.Name = txtName.Text;
            person.Age = (int)numAge.Value;
            person.Birthdate = dtpBirthdate.Value.Date;

            try
            {
                _context.SaveChanges();
                LoadPeople();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении записи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DeletePerson();
        }

        private void DeletePerson()
        {
            var idObj = dgvPeople.CurrentRow.Cells[0].Value;
            if (idObj == null) return;

            Guid id = (Guid)idObj;
            var confirm = MessageBox.Show("Вы уверены, что хотите удалить выбранную запись?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            var person = _context.People.Find(id);
            if (person == null) return;

            try
            {
                _context.People.Remove(person);
                _context.SaveChanges();
                LoadPeople();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtName.Text = string.Empty;
            numAge.Value = 0;
            dtpBirthdate.Value = DateTime.Today;
        }

        private void DgvPeople_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var idObj = dgvPeople.Rows[e.RowIndex].Cells[0].Value;
            if (idObj == null) return;

            Guid id = (Guid)idObj;
            var person = _context.People.FirstOrDefault(p => p.Id == id);
            if (person == null) return;

            txtName.Text = person.Name;
            numAge.Value = person.Age;
            dtpBirthdate.Value = person.Birthdate;

            dgvPeople.Rows[e.RowIndex].Selected = true;
        }
    }
}