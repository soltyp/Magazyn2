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
    public partial class FormUprawnienia : Form
    {
        private ZarzadzanieVoidami zarzadzanieVoidami;
        string StringPolaczeniowy = PolaczenieBazyDanych.StringPolaczeniowy();
        public FormUprawnienia()
        {
            InitializeComponent();
            zarzadzanieVoidami = new ZarzadzanieVoidami();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            string StringPolaczeniowy = PolaczenieBazyDanych.StringPolaczeniowy();
        }

        private void FormUprawnienia_Load(object sender, EventArgs e)
        {
            dataGridView2.Visible = true;
            dataGridView2.MaximumSize = new Size(420, dataGridView2.Height);
            WczytajDaneDoDataGridView2();
            OdswiezDataGridView();
            //comboBox1.SelectedIndex = 0;
            if (!ZarzadzanieVoidami.DodawanieRoli())
            {
                buttonDodajRole.Visible = false;
            }
            if (!ZarzadzanieVoidami.UsuwanieRoli())
            {
                buttonUsunRole.Visible = false;
            }
            if (!ZarzadzanieVoidami.EdytowanieRoli())
            {
                buttonEdytujRole.Visible = false;
            }
            label3.Visible = true;
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }
        private void WczytajDaneDoDataGridView2()
        {
            using (SqlConnection conn = new SqlConnection(StringPolaczeniowy))
            {
                conn.Open();
                string query = "SELECT * FROM dbo.Uprawnienia";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        DataTable transposedTable = new DataTable();

                        // Tworzenie jednej kolumny dla transponowanej tabeli
                        transposedTable.Columns.Add("Lista dostępnych uprawnien");

                        // Transpozycja - pomijamy dwie pierwsze kolumny
                        foreach (DataColumn col in dataTable.Columns)
                        {
                            if (dataTable.Columns.IndexOf(col) > 1) // Pomijamy dwie pierwsze kolumny
                            {
                                DataRow newRow = transposedTable.NewRow();
                                newRow[0] = col.ColumnName; // Ustawiamy nazwę kolumny jako wartość wiersza
                                transposedTable.Rows.Add(newRow);
                            }
                        }
                        dataGridView2.DataSource = transposedTable;
                    }
                }
            }
        }
        private void OdswiezDataGridView()
        {/*
            var listaUprawnien = zarzadzanieVoidami.PobierzUprawnienia();

            if (listaUprawnien.Count == 0)
            {
                MessageBox.Show("Lista uprawnień jest pusta.");
            }
            else
            {
                dataGridView1.DataSource = listaUprawnien;
                dataGridView1.Columns["UprawnienieID"].HeaderText = "ID Roli";
                dataGridView1.Columns["Nazwa_stanowiska"].HeaderText = "Nazwa Roli";
                dataGridView1.Columns["NadawanieRoli"].HeaderText = "Nadaj/Zmień Role";
            }*/
            string query = @"
            SELECT TOP (1000) 
                [UprawnienieID],
                [Nazwa_stanowiska],
                [DostepDoListyUzytkownikow],
                [DostepDoListyUprawnien],
                [DodawanieUzytkownika],
                [UsuwanieUzytkownika],
                [EdytowanieUzytkownika],
                [DodawanieRoli],
                [UsuwanieRoli],
                [EdytowanieRoli],
                [NadajZmienRoleStanowisko],
                [ZmienHaslo],
                [zmianaVAT],
                [PrzegladStanuMagazynowego],
                [PrzegladanieHistoriiStanuMagazynowego],
                [PrzegladHistoriiUzupelniania],
                [RejestracjaNowegoTowaru]
            FROM [MagazynTestowanieOprogramowania].[dbo].[Uprawnienia]";

            using (SqlConnection connection = new SqlConnection(StringPolaczeniowy))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            dataGridView1.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.Columns["UprawnienieID"].HeaderText = "ID Roli";
            dataGridView1.Columns["Nazwa_stanowiska"].HeaderText = "Nazwa Roli";
            dataGridView1.Columns["NadajZmienRoleStanowisko"].HeaderText = "Nadaj/Zmień Role";
            dataGridView1.Columns["DostepDoListyUzytkownikow"].HeaderText = "Dostep Do Listy Uzytkownikow";
            dataGridView1.Columns["DostepDoListyUprawnien"].HeaderText = "Dostep Do Listy Uprawnien";
            dataGridView1.Columns["DodawanieUzytkownika"].HeaderText = "Dodawanie Uzytkownika";
            dataGridView1.Columns["UsuwanieUzytkownika"].HeaderText = "Usuwanie Uzytkownika";
            dataGridView1.Columns["EdytowanieUzytkownika"].HeaderText = "Edytowanie Uzytkownika";
            dataGridView1.Columns["DodawanieRoli"].HeaderText = "Dodawanie Roli";
            dataGridView1.Columns["UsuwanieRoli"].HeaderText = "Usuwanie Roli";
            dataGridView1.Columns["EdytowanieRoli"].HeaderText = "Edytowanie Roli";
            dataGridView1.Columns["NadajZmienRoleStanowisko"].HeaderText = "Nadaj/Zmien Role/Stanowisko";
            dataGridView1.Columns["ZmienHaslo"].HeaderText = "Zmiana Hasla";
            dataGridView1.Columns["zmianaVAT"].HeaderText = "Zmiana stawki VAT";
            dataGridView1.Columns["PrzegladStanuMagazynowego"].HeaderText = "Przegląd stanów magazynowych (1,2)";
            dataGridView1.Columns["PrzegladanieHistoriiStanuMagazynowego"].HeaderText = "Przegladanie Historii Stanu Magazynowego na wskazana datę (3)";
            dataGridView1.Columns["PrzegladHistoriiUzupelniania"].HeaderText = "Przegladanie Historii Uzupełniania Stanów Magazynowych";
            dataGridView1.Columns["RejestracjaNowegoTowaru"].HeaderText = "Rejestracja Nowego Towaru/Produktu";









            dataGridView1.AutoResizeColumnHeadersHeight();
        }

        private void buttonDodajRole_Click(object sender, EventArgs e)
        {
            using (var formDodajRole = new FormDodajRole())
            {

                var result = formDodajRole.ShowDialog();

                if (result == DialogResult.OK)
                {
                    OdswiezDataGridView();
                }
            }
        }

        private void buttonUsunRole_Click(object sender, EventArgs e)
        {
            // Sprawdzenie czy został wybrany wiersz w dataGridView1
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Wybierz rolę do usunięcia.");
                return;
            }

            // Pobranie nazwy roli z zaznaczonego wiersza dataGridView1
            string nazwaRoli = dataGridView1.SelectedRows[0].Cells["Nazwa_stanowiska"].Value.ToString();

            // Wyświetlenie pytania z potwierdzeniem
            var result = MessageBox.Show("Czy na pewno chcesz usunąć rolę: " + nazwaRoli + "?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Zapytanie SQL do usunięcia roli z bazy danych
                string query = "DELETE FROM dbo.Uprawnienia WHERE Nazwa_stanowiska = @Nazwa";

                using (SqlConnection conn = new SqlConnection(StringPolaczeniowy))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@Nazwa", SqlDbType.NVarChar)).Value = nazwaRoli;
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Sprawdzenie czy rola została pomyślnie usunięta
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Rola została pomyślnie usunięta.");
                            // Odświeżenie dataGridView1
                            OdswiezDataGridView();
                        }
                        else
                        {
                            MessageBox.Show("Wystąpił błąd podczas usuwania roli.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Usuwanie roli zostało anulowane.");
            }

        }

        private void buttonEdytujRole_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Pobierz ID roli z zaznaczonego wiersza dataGridView1
                int roleId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["UprawnienieID"].Value);

                // Utwórz nową instancję formularza FormEdytujRole, przekazując ID roli
                using (var formEdytuj = new FormEdytujRole(roleId))
                {
                    var result = formEdytuj.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        OdswiezDataGridView();
                    }
                }
            }
            else
            {
                MessageBox.Show("Proszę zaznaczyć wiersz do edycji", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ZarzadzanieVoidami.CzyMaDostepDoListyUprawnien())
            {
                MessageBox.Show("Brak uprawnień w systemie.");
            }
            else
            {
                label3.Visible = true;
                dataGridView2.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            label3.Visible = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
