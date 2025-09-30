using Microsoft.EntityFrameworkCore;
using PeopleApp.Models;

namespace PeopleApp
{
    public partial class MainForm : Form
    {
        private readonly AppDbContext _context;
        private readonly DbContextOptions<AppDbContext> _options;

        private DataGridView dgvPeople;
        private TextBox txtName;
        private NumericUpDown numAge;
        private DateTimePicker dtpBirthdate;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;

        public MainForm(DbContextOptions<AppDbContext> options)
        {
            _options = options;
            _context = new AppDbContext(options);
            InitComponent();
        }

        private void InitComponent()
        {
            Text = "People Manager";
            Width = 800;
            Height = 600;

            dgvPeople = new DataGridView { Left = 10, Top = 10, Width = 760, Height = 300, ReadOnly = true, AutoGenerateColumns = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            dgvPeople.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id", DataPropertyName = "Id", Visible = false });
            dgvPeople.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Name", DataPropertyName = "Name", Width = 300 });
            dgvPeople.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Age", DataPropertyName = "Age", Width = 80 });
            dgvPeople.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Birthdate", DataPropertyName = "Birthdate", Width = 150 });

            var lblName = new Label { Text = "Name:", Left = 10, Top = 330, Width = 50 };
            txtName = new TextBox { Left = 70, Top = 325, Width = 300 };

            var lblAge = new Label { Text = "Age:", Left = 390, Top = 330, Width = 40 };
            numAge = new NumericUpDown { Left = 430, Top = 325, Width = 60, Minimum = 0, Maximum = 150 };

            var lblBirth = new Label { Text = "Birthdate:", Left = 510, Top = 330, Width = 70 };
            dtpBirthdate = new DateTimePicker { Left = 585, Top = 325, Width = 185, Format = DateTimePickerFormat.Short };

            btnAdd = new Button { Text = "Добавить", Left = 10, Top = 370, Width = 100 };
            btnUpdate = new Button { Text = "Обновить", Left = 120, Top = 370, Width = 100 };
            btnDelete = new Button { Text = "Удалить", Left = 230, Top = 370, Width = 100 };
            btnClear = new Button { Text = "Очистить", Left = 340, Top = 370, Width = 100 };

            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            btnClear.Click += BtnClear_Click;

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

        private void BtnAdd_Click(object sender, EventArgs e)
        {

        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
           
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {

        }

        private void BtnClear_Click(object sender, EventArgs e)
        {

        }
    }
}