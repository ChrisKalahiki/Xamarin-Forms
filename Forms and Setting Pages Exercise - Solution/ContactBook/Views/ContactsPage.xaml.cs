using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ContactBook
{
	public partial class ContactsPage : ContentPage
	{		
		private ObservableCollection<Contact> _contacts; 

		public ContactsPage()
		{
			InitializeComponent();

			_contacts = new ObservableCollection<Contact>
			{
				new Contact { Id = 1, FirstName = "John", LastName = "Smith", Email = "john@smith.com", Phone = "1111" },
				new Contact { Id = 2, FirstName = "Mary", LastName = "Johnson", Email = "mary@johnson.com", Phone = "2222" }
			};

			contacts.ItemsSource = _contacts;
		}

		async void OnAddContact(object sender, System.EventArgs e)
		{
			var page = new ContactDetailPage(new Contact());

			// We can subscribe to the ContactAdded event using a lambda expression.
			// If you're not familiar with this syntax, watch my C# Advanced course. 
			page.ContactAdded += (source, contact) =>
			{
				// ContactAdded event is raised when the user taps the Done button.
				// Here, we get notified and add this contact to our 
				// ObservableCollection.
				_contacts.Add(contact);
			};

			await Navigation.PushAsync(page);
		}

		async void OnContactSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			// We need to check if SelectedItem is null because further below 
			// we de-select the selected item. This will raise another ItemSelected
			// event, so this method will be called straight away. If we don't
			// check for null here, we'll get a NullReferenceException.
			if (contacts.SelectedItem == null)
				return; 

			var selectedContact = e.SelectedItem as Contact;

			// We de-select the selected item, so when we come back to the Contacts
			// page we can tap it again and navigate to ContactDetail. 
			contacts.SelectedItem = null;

			var page = new ContactDetailPage(selectedContact);
			page.ContactUpdated += (source, contact) =>
			{
				// When the target page raises ContactUpdated event, we get 
				// notified and update properties of the selected contact. 
				// Here we are dealing with a small class with only a few 
				// properties. If working with a larger class, you may want 
				// to look at AutoMapper, which is a convention-based mapping
				// tool. 
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
				_contacts.Remove(contact);
		}
	}
}
