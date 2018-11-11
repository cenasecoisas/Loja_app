using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Loja_app.modelsContext
{
    class Receita
    {
        //get sum
        public static double GetTotal(DateTime date)
        {
            double total = 0;
            using (var context = new LojaDBEntities())
            {
                var query = from b in context.Receita_Table
                            where b.Data.Value.Month == date.Month
                            select b;
                foreach (var receita in query)
                {
                    total += (double)receita.Valor;
                }

            }
            return total;
        }

        //get receita's table
        public static List<Receita_Table> GetListTable()
        {
            List<Receita_Table> receitaTable = new List<Receita_Table>();
            using (var context = new LojaDBEntities())
            {
                var query = from b in context.Receita_Table
                            select b;
                foreach (var receita in query)
                {
                    receitaTable.Add(receita);
                }

            }

            return receitaTable;
        }

        //generate data grid to be shown
        public static DataGrid GetDataGrid()
        {
            DataGrid dataGrid = new DataGrid
            {
                Name = "Receitas",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                ItemsSource = GetListTable()
            };

            return dataGrid;
        }

        public static void UpdateData(Receita_Table receita)
        {
            using (var context = new LojaDBEntities())
            {
                context.Entry(receita).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static void InsertData(Receita_Table receita)
        {
            using (var context = new LojaDBEntities())
            {
                context.Receita_Table.Add(receita);
                context.SaveChanges();
            }
        }

        //generate modal labels and inputs
        public static Grid GetInputModal(String action, Receita_Table table)
        {
            Grid gridPanel = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            //row's definition
            RowDefinition row = new RowDefinition();
            row.Height = GridLength.Auto;
            gridPanel.RowDefinitions.Add(row);

            RowDefinition row2 = new RowDefinition();
            row2.Height = GridLength.Auto;
            gridPanel.RowDefinitions.Add(row2);

            RowDefinition row3 = new RowDefinition();
            row3.Height = GridLength.Auto;
            gridPanel.RowDefinitions.Add(row3);

            //colunms definition
            ColumnDefinition col = new ColumnDefinition();
            col.Width = new GridLength(100, GridUnitType.Pixel);
            gridPanel.ColumnDefinitions.Add(col);

            ColumnDefinition col2 = new ColumnDefinition();
            col2.Width = new GridLength(150, GridUnitType.Pixel);
            gridPanel.ColumnDefinitions.Add(col2);

            //Label's definition
            Label labelValor = new Label();
            labelValor.Content = "Valor:";
            Grid.SetRow(labelValor, 0);
            Grid.SetColumn(labelValor, 0);

            Label labelData = new Label();
            labelData.Content = "Data:";
            Grid.SetRow(labelData, 1);
            Grid.SetColumn(labelData, 0);

            //Input's definition
            TextBox inputReceita = new TextBox();
            if (table != null)
                inputReceita.Text = table.Valor.ToString();
            inputReceita.TextAlignment = TextAlignment.Center;
            Grid.SetRow(inputReceita, 0);
            Grid.SetColumn(inputReceita, 1);

            DatePicker inputData = new DatePicker();
            Grid.SetRow(inputData, 1);
            Grid.SetColumn(inputData, 1);

            //button's definition
            Button cancelButton = new Button();
            cancelButton.Content = "Cancelar";
            cancelButton.Click += (sender, e) =>
            {
                Grid tempGrid = (Grid)gridPanel.Parent;
                Window tempWindow = (Window)tempGrid.Parent;
                tempWindow.Close();
            };
            Grid.SetRow(cancelButton, 2);
            Grid.SetColumn(cancelButton, 0);

            Button okButton = new Button();
            okButton.Content = "OK";
            okButton.Click += (sender, e) =>
            {
                int error = 0;
                Receita_Table receita = new Receita_Table();
                if (inputReceita.Text.Length > 0 && float.TryParse(inputReceita.Text, out float receitaValor))
                {
                    if (table != null)
                        table.Valor = receitaValor;
                    else
                        receita.Valor = receitaValor;
                }

                else error++;

                if (inputData.SelectedDate != null)
                {
                    if (table != null)
                        table.Data = inputData.SelectedDate;
                    else
                        receita.Data = inputData.SelectedDate;
                }

                else error++;

                if (error < 1)
                {
                    if (action == "Adicionar")
                        InsertData(receita);
                    if (action == "Editar")
                        UpdateData(table);

                    Grid tempGrid = (Grid)gridPanel.Parent;
                    Window tempWindow = (Window)tempGrid.Parent;
                    tempWindow.Close();

                }


            };
            Grid.SetRow(okButton, 2);
            Grid.SetColumn(okButton, 1);

            //grid's append
            gridPanel.Children.Add(labelValor);
            gridPanel.Children.Add(labelData);
            gridPanel.Children.Add(inputReceita);
            gridPanel.Children.Add(inputData);
            gridPanel.Children.Add(okButton);
            gridPanel.Children.Add(cancelButton);

            return gridPanel;
        }
    }
}
