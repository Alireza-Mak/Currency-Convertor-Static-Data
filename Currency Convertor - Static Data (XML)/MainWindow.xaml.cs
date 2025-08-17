using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Currency_Convertor_Static_Data
{
    /// <summary>
    /// This class represents the main window of the currency converter application.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ClearControls();
            BindData();
        }

        /// <summary>
        /// This method is used to create a DataTable and add static data to it, and then bind that data to the ComboBoxes for currency conversion.
        /// </summary>
        private void BindData()
        {
            //Create a new DataTable
            DataTable dtCurrency = new DataTable();

            //Add display column in DataTable
            dtCurrency.Columns.Add("Text");

            //Add value column in DataTable
            dtCurrency.Columns.Add("Value");
            dtCurrency.Rows.Add("--SELECT--", 0);
            dtCurrency.Rows.Add("INR", 87.52);
            dtCurrency.Rows.Add("USD", 1);
            dtCurrency.Rows.Add("EUR", 0.86);
            dtCurrency.Rows.Add("SAR", 3.75);
            dtCurrency.Rows.Add("GBP", 0.74);
            dtCurrency.Rows.Add("AUD", 1.53);
            dtCurrency.Rows.Add("CAD", 1.38);

            //The data to currency ComboBox is assigned from DataTable
            cmbFromCurrency.ItemsSource = dtCurrency.DefaultView;

            //DisplayMemberPath Property is used to display data in ComboBox
            cmbFromCurrency.DisplayMemberPath = "Text";

            //SelectedValuePath property is used to set the value in ComboBox
            cmbFromCurrency.SelectedValuePath = "Value";

            //SelectedIndex property is used to bind hint in the ComboBox. The default value is Select.
            cmbFromCurrency.SelectedIndex = 0;

            //All properties are set for 'To Currency' ComboBox as 'From Currency' ComboBox
            cmbToCurrency.ItemsSource = dtCurrency.DefaultView;
            cmbToCurrency.DisplayMemberPath = "Text";
            cmbToCurrency.SelectedValuePath = "Value";
            cmbToCurrency.SelectedIndex = 0;
        }

        /// <summary>
        /// This method is used to clear the controls in the UI, including the text box for currency input and the combo boxes for selecting currencies.
        /// </summary>
        private void ClearControls()
        {
            txtCurrency.Text = string.Empty;
            if (cmbFromCurrency.Items.Count > 0)
                cmbFromCurrency.SelectedIndex = 0;
            if (cmbToCurrency.Items.Count > 0)
                cmbToCurrency.SelectedIndex = 0;
            lblCurrency.Content = "";
            txtCurrency.Focus();
        }

        /// <summary>
        /// This method is used to validate the input in the currency text box to ensure that only numeric values are entered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        /// <summary>
        /// This method is used to handle the click event of the Clear button, which clears all controls in the UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
        }

        /// <summary>
        /// This method is used to handle the click event of the Convert button, which performs the currency conversion based on the selected currencies and the input amount.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Convert_Click(object sender, RoutedEventArgs e)
        {
            //Create a variable as ConvertedValue with double data type to store currency converted value
            double ConvertedValue;
            //Check amount textbox is Null or Blank
            if (txtCurrency.Text == null || txtCurrency.Text.Trim() == "")
            {
                //If amount textbox is Null or Blank it will show the below message box   
                MessageBox.Show("Please Enter Currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                //After clicking on message box OK sets the Focus on amount textbox
                txtCurrency.Focus();
                return;
            }
            //Else if the currency from is not selected or it is default text --SELECT--
            else if (cmbFromCurrency.SelectedValue == null || cmbFromCurrency.SelectedIndex == 0)
            {
                //It will show the message
                MessageBox.Show("Please Select Currency From", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                //Set focus on From ComboBox
                cmbFromCurrency.Focus();
                return;
            }
            //Else if Currency To is not Selected or Select Default Text --SELECT--
            else if (cmbToCurrency.SelectedValue == null || cmbToCurrency.SelectedIndex == 0)
            {
                //It will show the message
                MessageBox.Show("Please Select Currency To", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                //Set focus on To ComboBox
                cmbToCurrency.Focus();
                return;
            }

            //If From and To ComboBox selected values are same
            if (cmbFromCurrency.Text == cmbToCurrency.Text)
            {
                //The amount textbox value set in ConvertedValue.
                //double.parse is used to convert datatype String To Double.
                //Textbox text have string and ConvertedValue is double datatype
                ConvertedValue = double.Parse(txtCurrency.Text);

                //Show in label converted currency and converted currency name.
                // and ToString("N3") is used to place 000 after after the(.)
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
            else
            {
                //Calculation for currency converter is Tp Currency value multiply(*) 
                // with amount textbox value and then the total is divided(/) with From Currency value
                ConvertedValue = (double.Parse(cmbToCurrency.SelectedValue.ToString()) * double.Parse(txtCurrency.Text)) / double.Parse(cmbFromCurrency.SelectedValue.ToString());

                //Show in label converted currency and converted currency name.
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
        }

        /// <summary>
        ///  This method is used to handle the click event of the Swap button,
        ///  which swaps the selected currencies in the combo boxes for currency conversion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Swap_Click(object sender, RoutedEventArgs e)
        {
            if (cmbFromCurrency.Text == cmbToCurrency.Text)
            {
                Console.WriteLine(cmbFromCurrency.Text);
                Console.WriteLine(cmbToCurrency.Text);
                return;
            }
            (cmbFromCurrency.SelectedIndex, cmbToCurrency.SelectedIndex) = (cmbToCurrency.SelectedIndex, cmbFromCurrency.SelectedIndex);
        }

        /// <summary>
        /// This method is used to handle the text changed event of the currency input text box,
        /// formatting the input to include grouping separators for better readability.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnInputTextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox textBox = sender as TextBox;

            // Remove commas for parsing
            string raw = textBox.Text.Replace(",", "");

            // Split into integer + decimal parts
            string[] parts = raw.Split('.');
            if (decimal.TryParse(raw, out decimal _))
            {
                string intPart = parts[0];
                string decPart = parts.Length > 1 ? "." + parts[1] : "";

                // Add grouping separators only to integer part
                if (long.TryParse(intPart, out long intNumber))
                    textBox.Text = intNumber.ToString("N0") + decPart;
            }

            // Put caret at the end
            textBox.Focus();
            textBox.SelectionStart = textBox.Text.Length;
        }
    }
}
