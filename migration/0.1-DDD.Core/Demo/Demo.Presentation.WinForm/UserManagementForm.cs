using Demo.Presentation.WinForm.UserServiceReference;
using System;
using System.Windows.Forms;

namespace Demo.Presentation.WinForm
{
    public partial class UserManagementForm : Form
    {
        public UserManagementForm()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                IUserService service = new UserServiceClient();
                DemoUserDto userData = new DemoUserDto
                    {
                        UserName = txtUserName.Text,
                        PassWord = txtPassWord.Text,
                        Email = "test@163.com"
                    };

                DemoUserDto user = service.RegisterUserByDto(userData);
                MessageBox.Show(user.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                IUserService service = new UserServiceClient();
                string userName = txtUserName.Text;
                string passWord = txtPassWord.Text;
                DemoUserDto user = service.Login(userName, passWord);
                MessageBox.Show(user.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
