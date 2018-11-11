using Loja_app.modelsContext;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Loja_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const String PRODUTOS = "Produtos";
        const String DESPESAS = "Despesas";
        const String IVA = "IVA";
        const String RECEITAS = "Receitas";
        const String ADICIONAR = "Adicionar";
        const String EDITAR = "Editar";
        const String REMOVER = "Remover";
        const String RELATORIO = "Relatorio";

        public MainWindow()
        {
            InitializeComponent();

            List<String> dataStrings = new List<String>
            {
                PRODUTOS,
                DESPESAS,
                IVA,
                RECEITAS
            };

            List<String> actionStrings = new List<String>
            {
                ADICIONAR,
                EDITAR,
                REMOVER,
                RELATORIO
            };

            Dictionary<String, DateTime> months = new Dictionary<String, DateTime>();
            months.Add("Janeiro", new DateTime(2018, 1, 1));
            months.Add("Fevereio", new DateTime(2018, 2, 1));
            months.Add("Março", new DateTime(2018, 3, 1));
            months.Add("Abril", new DateTime(2018, 4, 1));
            months.Add("Maio", new DateTime(2018, 5, 1));
            months.Add("Junho", new DateTime(2018, 6, 1));
            months.Add("Julho", new DateTime(2018, 7, 1));
            months.Add("Agosto", new DateTime(2018, 8, 1));
            months.Add("Setembro", new DateTime(2018, 9, 1));
            months.Add("Outubro", new DateTime(2018, 10, 1));
            months.Add("Novembro", new DateTime(2018, 11, 1));
            months.Add("Dezembro", new DateTime(2018, 12, 1));

            TablesMenuGenerator(dataStrings);
            ActionsMenuGenerator(actionStrings, dataStrings, months);

        }

        //create action menu programmerly and dynamically
        private void ActionsMenuGenerator(List<String> actionStrings, List<String> dataStrings, Dictionary<String, DateTime> months)
        {
            foreach (var element in actionStrings)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = element;
                menuItem.Click += (sender, e) =>
                {
                    ModalGenerator(element, dataStrings, months);

                };
                actionsMenu.Items.Add(menuItem);
            }

            Grid modalGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch

            };
            RowDefinition row = new RowDefinition();
            row.Height = GridLength.Auto;
            modalGrid.RowDefinitions.Add(row);

            RowDefinition row2 = new RowDefinition();
            row2.Height = GridLength.Auto;
            modalGrid.RowDefinitions.Add(row2);

            CleanGrid(modalGrid);
            Grid gridPanel = new Grid();

        }

        private void ModalGenerator(String element, List<String> dataStrings, Dictionary<String, DateTime> months)
        {
            Window modal = new Window
            {
                Title = element,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };
            

            Grid modalGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch

            };
            RowDefinition row = new RowDefinition();
            row.Height = GridLength.Auto;
            modalGrid.RowDefinitions.Add(row);

            RowDefinition row2 = new RowDefinition();
            row2.Height = GridLength.Auto;
            modalGrid.RowDefinitions.Add(row2);

            CleanGrid(modalGrid);
            Grid gridPanel = new Grid();

            if (element == ADICIONAR)
            {
                ComboBox comboBox = new ComboBox();
                comboBox.ItemsSource = dataStrings;
                Grid.SetRow(comboBox, 0);
                modalGrid.Children.Add(comboBox);
                comboBox.SelectionChanged += (sender, e) =>
                {
                    if ((String)comboBox.SelectedItem == PRODUTOS)
                        gridPanel = Produto.GetInputModal(ADICIONAR, null);
                    if ((String)comboBox.SelectedItem == DESPESAS)
                        gridPanel = Despesa.GetInputModal(ADICIONAR, null);
                    if ((String)comboBox.SelectedItem == IVA)
                        gridPanel = Iva.GetInputModal(ADICIONAR, null);
                    if ((String)comboBox.SelectedItem == RECEITAS)
                        gridPanel = Receita.GetInputModal(ADICIONAR, null);

                    Grid.SetRow(gridPanel, 1);
                    modalGrid.Children.Add(gridPanel);
                };
                
                modal.Content = modalGrid;
                modal.ShowDialog();
            }

            if (element == EDITAR)
            {
                var mainGridColletion = mainGrid.Children;
                if(mainGridColletion != null)
                {
                    List<DataGrid> dataGrid = new List<DataGrid>();
                    foreach (var grid in mainGridColletion)
                    {
                        if (grid.GetType() == typeof(DataGrid))
                        {
                            dataGrid.Add((DataGrid)grid);
                        }
                    }

                    foreach (var grid in dataGrid)
                    {
                        if (grid.SelectedItem != null)
                        {
                            if (grid.Name == PRODUTOS)
                                gridPanel = Produto.GetInputModal(EDITAR, (Produto_Table)grid.SelectedItem);
                            if (grid.Name == DESPESAS)
                                gridPanel = Despesa.GetInputModal(EDITAR, (Despesas_Table)grid.SelectedItem);
                            if (grid.Name == IVA)
                                gridPanel = Iva.GetInputModal(EDITAR, (IVA_Table)grid.SelectedItem);
                            if (grid.Name == RECEITAS)
                                gridPanel = Receita.GetInputModal(EDITAR, (Receita_Table)grid.SelectedItem);
                        }
                    }

                    Grid.SetRow(gridPanel, 1);
                    modalGrid.Children.Add(gridPanel);
                    modal.Content = modalGrid;
                    modal.ShowDialog();
                }
                
            }

            if (element == REMOVER)
            {

            }

            if (element == RELATORIO)
            {
                ComboBox comboBox = new ComboBox();
                comboBox.ItemsSource = months.Keys;
                Grid.SetRow(comboBox, 0);
                modalGrid.Children.Add(comboBox);
                comboBox.SelectionChanged += (sender, e) =>
                {
                    if(modalGrid.Children.Count>1)
                        modalGrid.Children.RemoveAt(1);
                    months.TryGetValue(comboBox.SelectedItem.ToString(), out DateTime date);
                    gridPanel = GetReportModal(date);
                    Grid.SetRow(gridPanel, 1); 
                    modalGrid.Children.Add(gridPanel); 
                };
                modal.Content = modalGrid;
                modal.ShowDialog();
            }
        }

        private Grid GetReportModal(DateTime date)
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

            RowDefinition row8 = new RowDefinition();
            row8.Height = GridLength.Auto;
            gridPanel.RowDefinitions.Add(row8);

            RowDefinition row9 = new RowDefinition();
            row9.Height = GridLength.Auto;
            gridPanel.RowDefinitions.Add(row9);

            //colunms definition
            ColumnDefinition col = new ColumnDefinition();
            col.Width = new GridLength(150, GridUnitType.Pixel);
            gridPanel.ColumnDefinitions.Add(col);

            ColumnDefinition col2 = new ColumnDefinition();
            col2.Width = new GridLength(150, GridUnitType.Pixel);
            gridPanel.ColumnDefinitions.Add(col2);

            //Label's definition
            Label labelReceita = new Label();
            labelReceita.Content = "Total receita:";
            Grid.SetRow(labelReceita, 0);
            Grid.SetColumn(labelReceita, 0);

            Label labelReceitaTotal = new Label();
            labelReceitaTotal.Content = String.Format("{0:0.00}", Receita.GetTotal(date));
            Grid.SetRow(labelReceitaTotal, 0);
            Grid.SetColumn(labelReceitaTotal, 1);

            Label labelDespesaFactura = new Label();
            labelDespesaFactura.Content = "Despesa C/Factura:";
            Grid.SetRow(labelDespesaFactura, 1);
            Grid.SetColumn(labelDespesaFactura, 0);

            Label labelDespesaFacturaRetalho = new Label();
            labelDespesaFacturaRetalho.Content = "Retalho:";
            Grid.SetRow(labelDespesaFacturaRetalho, 2);
            Grid.SetColumn(labelDespesaFacturaRetalho, 0);

            Label labelDespesaFacturaRetalhoTotal = new Label();
            labelDespesaFacturaRetalhoTotal.Content = String.Format("{0:0.00}", Despesa.GetTotal(date)[0]);
            Grid.SetRow(labelDespesaFacturaRetalhoTotal, 2);
            Grid.SetColumn(labelDespesaFacturaRetalhoTotal, 1);

            Label labelDespesaFacturaSRetalho = new Label();
            labelDespesaFacturaSRetalho.Content = "S/Retalho:";
            Grid.SetRow(labelDespesaFacturaSRetalho, 3);
            Grid.SetColumn(labelDespesaFacturaSRetalho, 0);

            Label labelDespesaFacturaSRetalhoTotal = new Label();
            labelDespesaFacturaSRetalhoTotal.Content = String.Format("{0:0.00}", Despesa.GetTotal(date)[1]);
            Grid.SetRow(labelDespesaFacturaSRetalhoTotal, 3);
            Grid.SetColumn(labelDespesaFacturaSRetalhoTotal, 1);

            Label labelDespesaSFactura = new Label();
            labelDespesaSFactura.Content = "Despesa S/Factura:";
            Grid.SetRow(labelDespesaSFactura, 4);
            Grid.SetColumn(labelDespesaSFactura, 0);

            Label labelDespesaSFacturaRetalho = new Label();
            labelDespesaSFacturaRetalho.Content = "Retalho:";
            Grid.SetRow(labelDespesaSFacturaRetalho, 5);
            Grid.SetColumn(labelDespesaSFacturaRetalho, 0);

            Label labelDespesaSFacturaRetalhoTotal = new Label();
            labelDespesaSFacturaRetalhoTotal.Content = String.Format("{0:0.00}", Despesa.GetTotal(date)[2]);
            Grid.SetRow(labelDespesaSFacturaRetalhoTotal, 5);
            Grid.SetColumn(labelDespesaSFacturaRetalhoTotal, 1);

            Label labelDespesaSFacturaSRetalho = new Label();
            labelDespesaSFacturaSRetalho.Content = "S/Retalho:";
            Grid.SetRow(labelDespesaSFacturaSRetalho, 6);
            Grid.SetColumn(labelDespesaSFacturaSRetalho, 0);

            Label labelDespesaSFacturaSRetalhoTotal = new Label();
            labelDespesaSFacturaSRetalhoTotal.Content = String.Format("{0:0.00}", Despesa.GetTotal(date)[3]);
            Grid.SetRow(labelDespesaSFacturaSRetalhoTotal, 6);
            Grid.SetColumn(labelDespesaSFacturaSRetalhoTotal, 1);

            Label labelDespesas = new Label();
            labelDespesas.Content = "Total Despesa:";
            Grid.SetRow(labelDespesas, 7);
            Grid.SetColumn(labelDespesas, 0);

            Label labelDespesasTotal = new Label();
            labelDespesasTotal.Content = String.Format("{0:0.00}", Despesa.GetTotal(date)[4]);
            Grid.SetRow(labelDespesasTotal, 7);
            Grid.SetColumn(labelDespesasTotal, 1);

            //button's definition
            Button closeButton = new Button();
            closeButton.Content = "OK";
            closeButton.Click += (sender, e) =>
            {
                Grid tempGrid = (Grid)gridPanel.Parent;
                Window tempWindow = (Window)tempGrid.Parent;
                tempWindow.Close();
            };
            Grid.SetRow(closeButton, 8);
            Grid.SetColumn(closeButton, 0);

            //grid's append
            gridPanel.Children.Add(labelReceita);
            gridPanel.Children.Add(labelReceitaTotal);
            gridPanel.Children.Add(labelDespesaFactura);
            gridPanel.Children.Add(labelDespesaFacturaRetalho);
            gridPanel.Children.Add(labelDespesaFacturaRetalhoTotal);
            gridPanel.Children.Add(labelDespesaFacturaSRetalho);
            gridPanel.Children.Add(labelDespesaFacturaSRetalhoTotal);
            gridPanel.Children.Add(labelDespesaSFactura);
            gridPanel.Children.Add(labelDespesaSFacturaRetalho);
            gridPanel.Children.Add(labelDespesaSFacturaRetalhoTotal);
            gridPanel.Children.Add(labelDespesaSFacturaSRetalho);
            gridPanel.Children.Add(labelDespesaSFacturaSRetalhoTotal);
            gridPanel.Children.Add(labelDespesas);
            gridPanel.Children.Add(labelDespesasTotal);
            gridPanel.Children.Add(closeButton);

            return gridPanel;
        }

        //create table menu programmerly and dynamically
        private void TablesMenuGenerator(List<String> dataStrings)
        {
            foreach (var element in dataStrings)
            {
                MenuItem menuItem = new MenuItem
                {
                    Header = element
                };
                menuItem.Click += (sender, e) => 
                {
                    DataGrid dataGrid = new DataGrid();
                    if (element == PRODUTOS)
                    {
                        dataGrid = Produto.GetDataGrid();
                    }
                    if (element == DESPESAS)
                    {
                        dataGrid = Despesa.GetDataGrid();
                    }
                    if (element == IVA)
                    {
                        dataGrid = Iva.GetDataGrid();
                    }
                    if (element == RECEITAS)
                    {
                        dataGrid = Receita.GetDataGrid();
                    }
                    Grid.SetRow(dataGrid, 1);
                    CleanDataGrid();
                    mainGrid.Children.Add(dataGrid);
                };
                tablesMenu.Items.Add(menuItem);
            }
        }

        //clean data grids from main grid
        private void CleanDataGrid()
        {
            var dataGridColletion = mainGrid.Children;
            List<DataGrid> tempDataGrid = new List<DataGrid>();
            foreach (var element in dataGridColletion)
            {
                if (element.GetType() == typeof(DataGrid))
                {
                    tempDataGrid.Add((DataGrid)element);
                }
            }
            foreach (var element in tempDataGrid)
                mainGrid.Children.Remove(element);
        }

        //clean grid from modals
        private void CleanGrid(Grid grid)
        {
            var gridColletion = grid.Children;
            List<Grid> tempGrid = new List<Grid>();
            foreach (var element in gridColletion)
            {
                if (element.GetType() == typeof(Grid))
                {
                    tempGrid.Add((Grid)element);
                }
            }
            foreach (var element in tempGrid)
                grid.Children.Remove(element);
        }

    }
}
