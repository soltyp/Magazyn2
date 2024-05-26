﻿using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace TestowanieOprogramowania
{
    public partial class FormHUSM : Form
    {

        private ZarzadzanieVoidami zarzadzanieVoidami;
        string StringPolaczeniowy = PolaczenieBazyDanych.StringPolaczeniowy();

        public FormHUSM()
        {
            InitializeComponent();
            HUSM();
           
        }

        private void UstawSzerokoscKolumny()
        {
            // Pobierz bieżącą szerokość kolumny "Operacja"
            int currentWidth = dataGridView1.Columns["Operacja"].Width;

            // Oblicz nową szerokość jako 1.4 razy (czyli 2/5 większą) bieżącej szerokości
            int newWidth = (int)(currentWidth * 2);

            // Ustaw nową szerokość kolumny "Operacja"
            dataGridView1.Columns["Operacja"].Width = newWidth;
        }

        private void HUSM()
        {
            DateTime startDate = dateTimePickerStart1.Value.Date;
            DateTime endDate = dateTimePickerEnd.Value.Date;

            string query = @"SELECT TOP (1000) [ProduktID]
                  ,[NazwaTowaru]
                  ,[RodzajTowaru]
                  ,[JednostkaMiary]
                  ,[Ilosc]
                  ,[CenaNetto]
                  ,[StawkaVAT]
                  ,[Opis]
                  ,[Dostawca]
                  ,[DataDostawy]
                  ,[DataRejestracji]
                  ,[Rejestrujacy]
                  ,[DataZapisu]
                  ,[Operacja]
              FROM [MagazynTestowanieOprogramowania].[dbo].[ProduktyHistoriaOperacji]
              WHERE DataZapisu BETWEEN @StartDate AND @EndDate";

           
            using (SqlConnection connection = new SqlConnection(StringPolaczeniowy))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                   
                    DataTable dataTable = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }
        }

        private void SzukajProdukt_Click(object sender, EventArgs e)
        {
            HUSM();
        }

        private void FormHUSM_Load(object sender, EventArgs e)
        {
            UstawSzerokoscKolumny();

        }
    }
}
