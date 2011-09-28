using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace DataGridViewSample
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			dataGridView1.AutoGenerateColumns = false;

            Customer[] customers;

            customers = new Customer[3];

            customers[0] = new Customer();
            customers[0].FirstName = "Antonio";
            customers[0].LastName = "Bello";
            customers[0].Address.Street = "My Street";
            customers[0].Address.PostalCode = "32-500";
            customers[0].Address.City = "Chrzanow";

            customers[1] = new Customer();
            customers[1].FirstName = "Mike";
            customers[1].LastName = "Tester";
            customers[1].Address.Street = "His Street";
            customers[1].Address.PostalCode = "11223";
            customers[1].Address.City = "Rome";

            customers[2] = new Customer();
            customers[2].FirstName = "Eddie";
            customers[2].LastName = "Property";
            customers[2].Address.Street = "Pointer avenue";
            customers[2].Address.PostalCode = "55432";
            customers[2].Address.City = "London";

			dataGridView1.DataSource = customers;
		}

		private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if ((dataGridView1.Rows[e.RowIndex].DataBoundItem != null) && (dataGridView1.Columns[e.ColumnIndex].DataPropertyName.Contains(".")))
				e.Value = BindProperty(dataGridView1.Rows[e.RowIndex].DataBoundItem, dataGridView1.Columns[e.ColumnIndex].DataPropertyName);
		}

		private string BindProperty(object property, string propertyName)
		{
			string retValue;

			retValue = "";

			if (propertyName.Contains("."))
			{
				PropertyInfo[] arrayProperties;
				string leftPropertyName;

				leftPropertyName = propertyName.Substring(0, propertyName.IndexOf("."));
				arrayProperties = property.GetType().GetProperties();

				foreach (PropertyInfo propertyInfo in arrayProperties)
				{
					if (propertyInfo.Name == leftPropertyName)
					{
						retValue = BindProperty(propertyInfo.GetValue(property, null), propertyName.Substring(propertyName.IndexOf(".") + 1));
						break;
					}
				}
			}
			else
			{
				Type propertyType;
				PropertyInfo propertyInfo;

				propertyType = property.GetType();
				propertyInfo = propertyType.GetProperty(propertyName);
				retValue = propertyInfo.GetValue(property, null).ToString();
			}

			return retValue;
		}
	}

	public class Customer
	{
		private string firstName;
		private string lastName;
		private Address address = new Address ();

		public string FirstName { get { return firstName; } set { firstName = value; } }
		public string LastName { get { return lastName; } set { lastName = value; } }
		public Address Address { get { return address; } set { address = value; } }
	}

	public class Address
	{
		private string street;
		private string postalCode;
		private string city;

		public string Street { get { return street; } set { street = value; } }
		public string PostalCode { get { return postalCode; } set { postalCode = value; } }
		public string City { get { return city; } set { city = value; } }
	}
}