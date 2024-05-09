﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestowanieOprogramowania
{
    public partial class FormZmienHaslo : Form
    {
        string con = PolaczenieBazyDanych.StringPolaczeniowy();

        public FormZmienHaslo()
        {
            InitializeComponent();
            LoadUserLogins();
        }
        private void LoadUserLogins()
        {
            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT login FROM Uzytkownicy WHERE archiwizacja = 1", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBoxUsers.Items.Add(reader["login"].ToString());
                        }
                    }
                }
            }
        }
        public void ChangePassword(string login, string oldPassword, string newPassword)
        {
            int userId = GetUserIdByLogin(login);
            if (userId == -1)
            {
                MessageBox.Show("Nie znaleziono użytkownika.");
                return;
            }

            if (CheckPassword(userId, oldPassword))
            {
                if (oldPassword == newPassword)
                {
                    MessageBox.Show("Hasło musi się różnić od 3 ostatnich haseł.");
                }
                else if (CheckIfPasswordIsInHistory(userId, newPassword))
                {
                    MessageBox.Show("Hasło musi się różnić od 3 ostatnich haseł.");
                }
                else
                {
                    UpdatePassword(userId, newPassword);
                    MessageBox.Show("Hasło zostało zmienione.");
                    comboBoxUsers.SelectedIndex = -1;
                    textBoxNewPassword.Text = "";
                    textBoxOldPassword.Text = "";
                    textBoxNewPassword2.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Podane stare hasło jest nieprawidłowe.");
            }
        }

        private int GetUserIdByLogin(string login)
        {
            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT UzytkownikID FROM Uzytkownicy WHERE login = @login", connection))
                {
                    command.Parameters.AddWithValue("@login", login);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    return -1;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBoxUsers.SelectedItem == null)
            {
                MessageBox.Show("Proszę wybrać login użytkownika.");
                return;
            }
            string login = comboBoxUsers.SelectedItem.ToString();
            string oldPassword = textBoxOldPassword.Text;
            string newPassword = textBoxNewPassword.Text;
            string new2Password = textBoxNewPassword2.Text;

            if (!IsValidPassword(newPassword))
            {
                MessageBox.Show("Nowe hasło nie spełnia wymagań: musi mieć od 8 do 15 znaków, zawierać wielką literę, małą literę, cyfrę i jeden ze znaków specjalnych tj. -, _, !, *, #, $, &.");
                return;
            }

            if (newPassword == new2Password)
            {
                ChangePassword(login, oldPassword, newPassword);

            }
            else
            {
                MessageBox.Show("Nowe hasła nie są identyczne. Spróbuj ponownie.");
            }

        }

        private bool IsValidPassword(string password)
        {
            if (password.Length < 8 || password.Length > 15)
                return false;

            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;
            bool hasSpecial = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                if (char.IsLower(c)) hasLower = true;
                if (char.IsDigit(c)) hasDigit = true;
                if ("-_!*$&#".Contains(c)) hasSpecial = true;
            }

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }
        private bool CheckPassword(int userId, string oldPassword)
        {
            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT haslo FROM Uzytkownicy WHERE UzytkownikID = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", userId);

                    var storedPassword = command.ExecuteScalar() as string;
                    return storedPassword == oldPassword;
                }
            }
        }
        private void UpdatePassword(int userId, string newPassword)
        {
            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE Uzytkownicy SET haslo = @newPassword WHERE UzytkownikID = @id", connection))
                {
                    command.Parameters.AddWithValue("@newPassword", newPassword);
                    command.Parameters.AddWithValue("@id", userId);
                    command.ExecuteNonQuery();
                }
            }
        }
        private bool CheckIfPasswordIsInHistory(int userId, string newPassword)
        {
            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT haslo1, haslo2, haslo3 FROM HistoriaHasel WHERE UzytkownikID = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string historyPassword1 = reader["haslo1"] as string;
                            string historyPassword2 = reader["haslo2"] as string;
                            string historyPassword3 = reader["haslo3"] as string;

                            return (newPassword == historyPassword1 || newPassword == historyPassword2 || newPassword == historyPassword3);
                        }
                    }
                }
            }
            return false;
        }

        private void FormZmienHaslo_Load(object sender, EventArgs e)
        {

        }
    }
}
