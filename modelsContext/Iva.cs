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
    class Iva
    {

        //get despesas's table
        public static List<IVA_Table> GetListTable()
        {
            List<IVA_Table> despesasTable = new List<IVA_Table>();
            using (var context = new LojaDBEntities())
            {
                var query = from b in context.IVA_Table
                            select b;
                foreach (var iva in query)
                {
                    despesasTable.Add(iva);
                }

            }

            return despesasTable;
        }

        //generate data grid to be shown
        public static DataGrid GetDataGrid()
        {
            DataGrid dataGrid = new DataGrid
            {
                Name = "IVA",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                ItemsSource = GetListTable()
            };

            return dataGrid;
        }

        //generate modal labels and inputs
        public static Grid GetInputModal(String action, IVA_Table table)
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
            Label labelIva = new Label();
            labelIva.Content = "IVA:";
            Grid.SetRow(labelIva, 0);
            Grid.SetColumn(labelIva, 0);

            Label labelDescricao = new Label();
            labelDescricao.Content = "Descrição";
            Grid.SetRow(labelDescricao, 1);
            Grid.SetColumn(labelDescricao, 0);

            //Input's definition
            TextBox inputIva = new TextBox();
            if (table != null)
                inputIva.Text = table.IVA.ToString();
            inputIva.TextAlignment = TextAlignment.Center;
            Grid.SetRow(inputIva, 0);
            Grid.SetColumn(inputIva, 1);

            TextBox inputDescricao = new TextBox();
            if (table != null)
                inputDescricao.Text = table.Descrição.ToString();
            inputDescricao.TextAlignment = TextAlignment.Center;
            Grid.SetRow(inputDescricao, 1);
            Grid.SetColumn(inputDescricao, 1);

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
                IVA_Table iva = new IVA_Table();
                if (inputIva.Text.Length > 0 && int.TryParse(inputIva.Text, out int ivaValor))
                {
                    if (table != null)
                        table.IVA = ivaValor;
                    else
                        iva.IVA = ivaValor;
                }
                    
                else error++;

                if (inputDescricao.Text.Length > 0)
                {
                    if (table != null)
                        table.Descrição = inputDescricao.Text;
                    else
                        iva.Descrição = inputDescricao.Text;
                }
                    
                else error++;

                if (error < 1)
                {
                    if (action == "Adicionar")
                        InsertData(iva);
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
            gridPanel.Children.Add(labelIva);
            gridPanel.Children.Add(labelDescricao);
            gridPanel.Children.Add(inputIva);
            gridPanel.Children.Add(inputDescricao);
            gridPanel.Children.Add(okButton);
            gridPanel.Children.Add(cancelButton);

            return gridPanel;
        }

        public static void UpdateData(IVA_Table iva)
        {
            using (var context = new LojaDBEntities())
            {
                context.Entry(iva).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static void InsertData(IVA_Table iva)
        {
            using (var context = new LojaDBEntities())
            {
                context.IVA_Table.Add(iva);
                context.SaveChanges();
            }
        }
    }
}
