using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ContactBook.Persistence;
using SQLite;
using Xamarin.Forms;

namespace ContactBook
{
	public partial class ContactsPage : ContentPage
	{		
		private ObservableCollection<Contact> _contacts;
		private SQLiteAsyncConnection _connection;
		private bool _isDataLoaded;

		public ContactsPage()
		{
			InitializeComponent();

			_connection = DependencyService.Get<ISQLiteDb>().GetConnection();
		}

		protected override async void OnAppearing()
		{
			// In a multi-page app, everytime we come back to this page, OnAppearing
			// method is called, but we want to load the data only the first time
			// this page is loaded. In other words, when we go to ContactDetailPage
			// and come back, we don't want to reload the data. The data is already
			// there. We can control this using a switch: isDataLoaded.
			if (_isDataLoaded)
				return;

			_isDataLoaded = true; 

			// I've extracted the logic for loading data into LoadData method. 
			// Now the code in OnAppearing method looks a lot cleaner. The 
			// purpose is very explicit. If data is loaded, return, otherwise,
			// load data. Details of loading the data is delegated to LoadData
			// method. 
			await LoadData();

			base.OnAppearing();
		}

		// Note that this method returns a Task, instead of void. Void should 
		// only be used for event handlers (e.g. OnAppearing). In all other cases,
		// you should return a Task or Task<T>.
		private async Task LoadData()
		{
			await _connection.CreateTableAsync<Contact>();

			var contacts = await _connection.Table<Contact>().ToListAsync();

			_contacts = new ObservableCollection<Contact>(contacts);
			contactsListView.ItemsSource = _contacts;
		}

		async void OnAddContact(object sender, System.EventArgs e)
		{
			var page = new ContactDetailPage(new Contact());

			page.ContactAdded += (source, contact) =>
			{
				_contacts.Add(contact);
			};

			await Navigation.PushAsync(page);
		}

		async void OnContactSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			if (contactsListView.SelectedItem == null)
				return; 

			var selectedContact = e.SelectedItem as Contact;

			contactsListView.SelectedItem = null;

			var page = new ContactDetailPage(selectedContact);
			page.ContactUpdated += (source, contact) =>
			{
				selectedContact.Id = contact.Id; 
				selectedContact.FirstName = contact.FirstName;
				selectedContact.LastName = contact.LastName;
				selectedContact.Phone = contact.Phone;
				selectedContact.Email = contact.Email;
				selectedContact.IsBlocked = contact.IsBlocked;
			};

			await Navigation.PushAsync(page);
		}

		async void OnDeleteContact(object sender, System.EventArgs e)
		{
			var contact = (sender as MenuItem).CommandParameter as Contact;

			if (await DisplayAlert("Warning", $"Are you sure you want to delete {contact.FullName}?", "Yes", "No")) 
			{
				_contacts.Remove(contact);

				await _connection.DeleteAsync(contact);
			}
		}
	}
}
