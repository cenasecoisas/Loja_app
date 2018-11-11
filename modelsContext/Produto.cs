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
    class Produto
    {

        //get produto's table
        public static List<Produto_Table> GetListTable()
        {
            List<Produto_Table> produtoTable = new List<Produto_Table>();
            using (var context = new LojaDBEntities())
            {
                var query = from b in context.Produto_Table
                            select b;
                foreach (var produto in query)
                {
                    produtoTable.Add(produto);
                }

            }

            return produtoTable;
        }

        //generate data grid to be shown
        public static DataGrid GetDataGrid()
        {
            DataGrid dataGrid = new DataGrid
            {
                Name = "Produtos",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                ItemsSource = GetListTable()
            };

            return dataGrid;
        }

        //generate modal labels and inputs
        public static Grid GetInputModal(String action, Produto_Table table)
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

            Label labelIva = new Label();
            labelIva.Content = "IVA";
            Grid.SetRow(labelIva, 1);
            Grid.SetColumn(labelIva, 0);

            Label labelCompra = new Label();
            labelCompra.Content = "Valor de Compra:";
            Grid.SetRow(labelCompra, 2);
            Grid.SetColumn(labelCompra, 0);

            Label labelFinal = new Label();
            labelFinal.Content = "Valor Final:";
            Grid.SetRow(labelFinal, 3);
            Grid.SetColumn(labelFinal, 0);

            //Input's definition
            TextBox inputNome = new TextBox();
            if (table != null)
                inputNome.Text = table.Nome;
            inputNome.TextAlignment = TextAlignment.Center;
            Grid.SetRow(inputNome, 0);
            Grid.SetColumn(inputNome, 1);

            ComboBox comboBoxIva = new ComboBox();
            List<String> IvaList = new List<String>();
            foreach (var element in Iva.GetListTable())
            {
                IvaList.Add(element.IVA.ToString());
            }
            comboBoxIva.ItemsSource = IvaList;
            if (table != null)
                comboBoxIva.SelectedIndex = (int)table.IVA-1;
            Grid.SetRow(comboBoxIva, 1);
            Grid.SetColumn(comboBoxIva, 1);

            TextBox inputCompra = new TextBox();
            if (table != null)
                inputCompra.Text = table.Valor_compra.ToString();
            inputCompra.TextAlignment = TextAlignment.Center;
            Grid.SetRow(inputCompra, 2);
            Grid.SetColumn(inputCompra, 1);

            TextBox inputFinal = new TextBox();
            if (table != null)
                inputFinal.Text = table.Valor_final.ToString();
            inputFinal.TextAlignment = TextAlignment.Center;
            Grid.SetRow(inputFinal, 3);
            Grid.SetColumn(inputFinal, 1);

            //button's definition
            Button cancelButton = new Button();
            cancelButton.Content = "Cancelar";
            cancelButton.Click += (sender, e) =>
            {
                Grid tempGrid = (Grid)gridPanel.Parent;
                Window tempWindow = (Window)tempGrid.Parent;
                tempWindow.Close();
            };
            Grid.SetRow(cancelButton, 4);
            Grid.SetColumn(cancelButton, 0);

            Button okButton = new Button();
            okButton.Content = "OK";
            okButton.Click += (sender, e) =>
            {
                int error = 0;
                Produto_Table produto = new Produto_Table();
                if (inputNome.Text.Length > 0)
                    if (table != null)
                        table.Nome = inputNome.Text;
                    else
                        produto.Nome = inputNome.Text;
                else error++;

                if (comboBoxIva.SelectedItem != null)
                    if (table != null)
                        table.IVA = ValidateIVA(int.Parse(comboBoxIva.SelectedItem.ToString()));
                    else
                        produto.IVA = ValidateIVA(int.Parse(comboBoxIva.SelectedItem.ToString()));
                else error++;

                if (inputCompra.Text.Length > 0 && float.TryParse(inputCompra.Text, out float valorCompra))
                    if (table != null)
                        table.Valor_compra = valorCompra;
                    else
                        produto.Valor_compra = valorCompra;
                else error++;

                if (inputFinal.Text.Length > 0 && float.TryParse(inputFinal.Text, out float valorFinal))
                    if (table != null)
                        table.Valor_final = valorFinal;
                    else
                        produto.Valor_final = valorFinal;
                else error++;

                if (error < 1)
                {
                    if (action == "Adicionar")
                        InsertData(produto);
                    if (action == "Editar")
                        UpdateData(table);

                    Grid tempGrid = (Grid)gridPanel.Parent;
                    Window tempWindow = (Window)tempGrid.Parent;
                    tempWindow.Close();
                }
                    
                
            };
            Grid.SetRow(okButton, 4);
            Grid.SetColumn(okButton, 1);

            //grid's append
            gridPanel.Children.Add(labelNome);
            gridPanel.Children.Add(labelIva);
            gridPanel.Children.Add(labelCompra);
            gridPanel.Children.Add(labelFinal);
            gridPanel.Children.Add(inputNome);
            gridPanel.Children.Add(comboBoxIva);
            gridPanel.Children.Add(inputCompra);
            gridPanel.Children.Add(inputFinal);
            gridPanel.Children.Add(okButton);
            gridPanel.Children.Add(cancelButton);

            return gridPanel;
        }

        public static void UpdateData(Produto_Table produto)
        {
            using (var context = new LojaDBEntities())
            {
                context.Entry(produto).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static void InsertData(Produto_Table produto)
        {
            using (var context = new LojaDBEntities())
            {
                context.Produto_Table.Add(produto);
                context.SaveChanges();
            }
        }

        public static int ValidateIVA(int iva)
        {
            int ivaId = 0;
            using (var context = new LojaDBEntities())
            {
                var query = from b in context.IVA_Table
                            where b.IVA == iva
                            select b.ID;
                foreach (int element in query)
                {
                    ivaId = element;
                }
            }

            return ivaId;
        }

    }
}
