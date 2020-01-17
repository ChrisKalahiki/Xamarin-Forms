using System;

using Xamarin.Forms;

namespace ContactBook
{
	public partial class ContactDetailPage : ContentPage
	{
		// EventHandler<Contact> is a delegate that can reference a method with 
		// the following signature:
		//
		// void MethodName(object source, Contact args); 
		// 
		// If you are not familiar with EventHandler, take my C# Advanced course. 
		public event EventHandler<Contact> ContactAdded;
		public event EventHandler<Contact> ContactUpdated;

		public ContactDetailPage(Contact contact)
		{
			// Note the use of nameof() operator in C# 6. 
			if (contact == null)
				throw new ArgumentNullException(nameof(contact));
			
			InitializeComponent();

			// We do not use the given contact as the BindingContext. 
			// The reason for this is that if the user make changes and
			// clicks the back button (instead of Save), the changes 
			// should be reverted. So, we create a separate Contact object
			// based on the given Contact and use that temporarily on this
			// page. When Save is clicked, we raise an event and notify 
			// others (in this case ContactsPage) that a contact is added or 
			// updated.
			BindingContext = new Contact
			{
				Id = contact.Id, 
				FirstName = contact.FirstName,
				LastName = contact.LastName,
				Phone = contact.Phone,
				Email = contact.Email,
				IsBlocked = contact.IsBlocked
			};
		}

		async void OnSave(object sender, System.EventArgs e)
		{
			var contact = BindingContext as Contact;

			if (String.IsNullOrWhiteSpace(contact.FullName)) 
			{
				await DisplayAlert("Error", "Please enter the name.", "OK");
				return;
			}

			if (contact.Id == 0)
			{
				// This is just a temporary hack to differentiate between a
				// new and an existing Contact object. In the next section, 
				// we'll store these Contact objects in a database. So, they
				// will automaticlaly get an Id.
				contact.Id = 1;

				// This is null-conditional operator in C#. It is the same as:
				// 
				// if (ContactAdded != null)
				// 		ContactAdded(this, contact);
				//
				// Read my blog post for more details:
				// http://programmingwithmosh.com/csharp/csharp-6-features-that-help-you-write-cleaner-code/
				//
				ContactAdded?.Invoke(this, contact);
			} 
			else
			{
				ContactUpdated?.Invoke(this, contact);
			}

			await Navigation.PopAsync();
		}
	}
}
