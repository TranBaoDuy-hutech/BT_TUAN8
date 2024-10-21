using BINDING.MODEL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BINDING
{
    public partial class Form1 : Form
    {
        private BindingSource bindingSource = new BindingSource();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (StudentModel dbContext = new StudentModel())
            {
                var students = dbContext.Students.ToList();
                bindingSource.DataSource = students;
                dataGridView1.DataSource = bindingSource;
            
                textBox2.DataBindings.Add("Text", bindingSource, "FullName");
                textBox3.DataBindings.Add("Text", bindingSource, "Age");
                comboBox1.DataBindings.Add("Text", bindingSource, "Major");

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (StudentModel dbContext = new StudentModel())
                {
                    // Tạo sinh viên mới
                    Students newStudent = new Students
                    {
                        FullName = textBox2.Text,
                        Age = int.Parse(textBox3.Text),
                        Major = comboBox1.Text
                    };

                    // Thêm vào DbContext
                    dbContext.Students.Add(newStudent);

                    // Lưu thay đổi xuống cơ sở dữ liệu
                    dbContext.SaveChanges();

                    // Cập nhật BindingSource với sinh viên mới
                    bindingSource.Add(newStudent);
                    MessageBox.Show("Thêm sinh viên thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sinh viên: " + ex.Message);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (StudentModel dbContext = new StudentModel())
                {
                    // Lấy sinh viên hiện tại từ BindingSource
                    Students currentStudent = (Students)bindingSource.Current;

                    if (currentStudent != null)
                    {
                        // Đính kèm đối tượng hiện tại vào DbContext nếu nó không thuộc về DbContext
                        dbContext.Students.Attach(currentStudent);

                        // Xóa sinh viên khỏi DbContext
                        dbContext.Students.Remove(currentStudent);

                        // Lưu thay đổi xuống cơ sở dữ liệu
                        dbContext.SaveChanges();

                        // Cập nhật lại BindingSource
                        bindingSource.RemoveCurrent();
                        MessageBox.Show("Xóa sinh viên thành công!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sinh viên: " + ex.Message);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (StudentModel dbContext = new StudentModel())
                {
                    // Lấy sinh viên hiện tại từ BindingSource
                    Students currentStudent = (Students)bindingSource.Current;

                    if (currentStudent != null)
                    {
                        // Đính kèm đối tượng hiện tại vào DbContext nếu nó không thuộc về DbContext
                        dbContext.Students.Attach(currentStudent);

                        // Cập nhật các trường cần sửa
                        currentStudent.FullName = textBox2.Text;
                        currentStudent.Age = int.Parse(textBox3.Text);  // Đảm bảo đúng kiểu dữ liệu
                        currentStudent.Major = comboBox1.Text;

                        // Đánh dấu đối tượng là đã sửa đổi
                        dbContext.Entry(currentStudent).State = System.Data.Entity.EntityState.Modified;

                        // Lưu thay đổi vào cơ sở dữ liệu
                        dbContext.SaveChanges();

                        MessageBox.Show("Cập nhật sinh viên thành công!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật sinh viên: " + ex.Message);
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            bindingSource.MoveNext();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bindingSource.MovePrevious();

        }
    }
}
