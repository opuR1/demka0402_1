using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Service
{
    internal class AuthorizationService
    {
        public Users _user = null;
        public Users GetUser(string password, string login)
        {
            if(password == null)
            {
                throw new ArgumentNullException("Введите пароль!");
            }
            if (login == null)
            {
                throw new ArgumentNullException("Введите логин!");
            }

            using (var db = kr_de1Entities.GetContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
                if (user == null)
                {
                    throw new Exception("Неверный логин или пароль!");
                }
                _user = user;
                return _user;
            }
        }
        public void ClearUser()
        {
            _user = null;
        }
    }
}
