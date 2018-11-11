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
    class Despesa
    {

        //get sum
        public static double[] GetTotal(DateTime date)
        {
            double[] totais = new double[5];
            using (var context = new LojaDBEntities())
            {
                var query = from b in context.Despesas_Table
                            where b.Data.Value.Month == date.Month
                            select b;
                foreach (var despesa in query)
                {
                    if(despesa.Factura == true && despesa.Retalho == true)
                        totais[0] += (double)despesa.Valor;
                    if (despesa.Factura == true && despesa.Retalho == false)
                        totais[1] += (double)despesa.Valor;
                    if (despesa.Factura == false && despesa.Retalho == true)
                        totais[2] += (double)despesa.Valor;
                    if (despesa.Factura == false && despesa.Retalho == false)
                        totais[3] += (double)despesa.Valor;

                }
                for (int i = 0; i < totais.Length - 1; i++)
                {
                    totais[totais.Length - 1] += totais[i];
                }

            }
            return totais;
        }

        //get despesas's table
        public static List<Despesas_Table> GetListTable()
        {
            List<Despesas_Table> despesasTable = new List<Despesas_Table>();
            using (var context = new LojaDBEntities())
            {
                var query = from b in context.Despesas_Table
                            select b;
                foreach (var despesa in query)
                {
                    despesasTable.Add(despesa);
                }

            }

            return despesasTable;
        }

        //generate data grid to be shown
        public static DataGrid GetDataGrid()
        {
            DataGrid dataGrid = new DataGrid
            {
                Name = "Despesas",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                ItemsSource = GetListTable()
            };

            return dataGrid;
        }

        //generate modal labels and inputs
        public static Grid GetInputModal(String action, Despesas_Table table)
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

            RowDefinition row4 = new RowDefinition();
            row4.Height = GridLength.Auto;
            gridPanel.RowDefinitions.Add(row4);

            RowDefinition row5 = new RowDefinition();
            row5.Height = GridLength.Auto;
            gridPanel.RowDefinitions.Add(row5);

            RowDefinition row6 = new RowDefinition();
            row6.Height = GridLength.Auto;
            gridPanel.RowDefinitions.Add(row6);

            RowDefinition row7 = new RowDefinition();
            row7.Height = GridLength.Auto;
            gridPanel.RowDefinitions.Add(row7);

            //colunms definition
            ColumnDefinition col = new ColumnDefinition();
            col.Width = new GridLength(100, GridUnitType.Pixel);
            gridPanel.ColumnDefinitions.Add(col);

            ColumnDefinition col2 = new ColumnDefinition();
            col2.Width = new GridLength(150, GridUnitType.Pixel);
            gridPanel.ColumnDefinitions.Add(col2);

            //Label's definition
            Label labelNome = new Label();
            labelNome.Content = "Nome:";
            Grid.SetRow(labelNome, 0);
            Grid.SetColumn(labelNome, 0);

            Label labelDescricao = new Label();
            labelDescricao.Content = "Descrição";
            Grid.SetRow(labelDescricao, 1);
            Grid.SetColumn(labelDescricao, 0);

            Label labelFactura = new Label();
            labelFactura.Content = "Factura:";
            Grid.SetRow(labelFactura, 2);
            Grid.SetColumn(labelFactura, 0);

            Label labelRetalho = new Label();
            labelRetalho.Content = "Retalho:";
            Grid.SetRow(labelRetalho, 3);
            Grid.SetColumn(labelRetalho, 0);

            Label labelValor = new Label();
            labelValor.Content = "Valor:";
            Grid.SetRow(labelValor, 4);
            Grid.SetColumn(labelValor, 0);

            Label labelData = new Label();
            labelData.Content = "Data:";
            Grid.SetRow(labelData, 5);
            Grid.SetColumn(labelData, 0);

            //Input's definition
            TextBox inputNome = new TextBox();
            if (table != null)
                inputNome.Text = table.Nome;
            inputNome.TextAlignment = TextAlignment.Center;
            Grid.SetRow(inputNome, 0);
            Grid.SetColumn(inputNome, 1);

            TextBox inputDescricao = new TextBox();
            if (table != null)
                inputDescricao.Text = table.Descrição;
            inputDescricao.TextAlignment = TextAlignment.Center;
            Grid.SetRow(inputDescricao, 1);
            Grid.SetColumn(inputDescricao, 1);

            CheckBox inputFactura = new CheckBox();
            if (table != null)
                inputFactura.IsChecked = table.Factura;
            Grid.SetRow(inputFactura, 2);
            Grid.SetColumn(inputFactura, 1);

            CheckBox inputRetalho = new CheckBox();
            if (table != null)
                inputRetalho.IsChecked = table.Retalho;
            Grid.SetRow(inputRetalho, 3);
            Grid.SetColumn(inputRetalho, 1);

            TextBox inputValor = new TextBox();
            if (table != null)
                inputValor.Text = table.Valor.ToString();
            inputValor.TextAlignment = TextAlignment.Center;
            Grid.SetRow(inputValor, 4);
            Grid.SetColumn(inputValor, 1);

            DatePicker inputData = new DatePicker();
            Grid.SetRow(inputData, 5);
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
            Grid.SetRow(cancelButton, 6);
            Grid.SetColumn(cancelButton, 0);

            Button okButton = new Button();
            okButton.Content = "OK";
            okButton.Click += (sender, e) =>
            {
                int error = 0;
                Despesas_Table despesa = new Despesas_Table();
                if (inputNome.Text.Length > 0)
                    if (table != null)
                        table.Nome = inputNome.Text;
                    else
                        despesa.Nome = inputNome.Text;
                else error++;

                if (inputDescricao.Text.Length > 0)
                {
                    if (table != null)
                        table.Descrição = inputDescricao.Text;
                    else
                        despesa.Descrição = inputDescricao.Text;
                }
                else error++;

                if (table != null)
                    table.Factura = inputFactura.IsChecked;
                else
                    despesa.Factura = inputFactura.IsChecked;

                if(table != null)
                    table.Retalho = inputRetalho.IsChecked;
                else
                    despesa.Retalho = inputRetalho.IsChecked;

                if (inputValor.Text.Length > 0 && float.TryParse(inputValor.Text, out float valor))
                    if (table != null)
                        table.Valor = valor;
                    else
                        despesa.Valor = valor;
                else error++;

                if (inputData.SelectedDate != null)
                    if (table != null)
                        table.Data = inputData.SelectedDate;
                    else
                        despesa.Data = inputData.SelectedDate;

                if (error < 1)
                {
                    if (action == "Adicionar")
                        InsertData(despesa);
                    if (action == "Editar")
                        UpdateData(table);

                    Grid tempGrid = (Grid)gridPanel.Parent;
                    Window tempWindow = (Window)tempGrid.Parent;
                    tempWindow.Close();
                }


            };
            Grid.SetRow(okButton, 6);
            Grid.SetColumn(okButton, 1);

            //grid's append
            gridPanel.Children.Add(labelNome);
            gridPanel.Children.Add(labelDescricao);
            gridPanel.Children.Add(labelFactura);
            gridPanel.Children.Add(labelRetalho);
            gridPanel.Children.Add(labelValor);
            gridPanel.Children.Add(labelData);
            gridPanel.Children.Add(inputNome);
            gridPanel.Children.Add(inputDescricao);
            gridPanel.Children.Add(inputFactura);
            gridPanel.Children.Add(inputRetalho);
            gridPanel.Children.Add(inputValor);
            gridPanel.Children.Add(inputData);
            gridPanel.Children.Add(okButton);
            gridPanel.Children.Add(cancelButton);

            return gridPanel;
        }

        public static void UpdateData(Despesas_Table despesa)
        {
            using (var context = new LojaDBEntities())
            {
                context.Entry(despesa).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static void InsertData(Despesas_Table despesa)
        {
            using (var context = new LojaDBEntities())
            {
                context.Despesas_Table.Add(despesa);
                context.SaveChanges();
            }
        }
    }
}
