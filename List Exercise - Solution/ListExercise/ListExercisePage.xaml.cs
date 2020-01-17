using System.Collections.Generic;
using ListExercise.Models;
using ListExercise.Services;
using Xamarin.Forms;

namespace ListExercise
{
	public partial class ListExercisePage : ContentPage
	{
		private SearchService _searchService;
		private List<SearchGroup> _searchGroups;

		public ListExercisePage()
		{
			_searchService = new SearchService();

			InitializeComponent();

			PopulateListView(_searchService.GetRecentSearches());
		}

		private void OnSearchTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			PopulateListView(_searchService.GetRecentSearches(e.NewTextValue));
		}

		// Note that we have re-used this method twice in this class. I noticed
		// I always had to set _searchGroups and immediately set listView.ItemsSource
		// together. So, I decided to refactor these few lines into a separate
		// method (PopulateListView) to make the code cleaner. 
		private void PopulateListView(IEnumerable<Search> searches)
		{
			_searchGroups = new List<SearchGroup>
			{
				new SearchGroup("Recent Searches", searches)
			};

			listView.ItemsSource = _searchGroups;
		}

		private void OnDeleteClicked(object sender, System.EventArgs e)
		{
			var search = (sender as MenuItem).CommandParameter as Search;

			// Locally remove the search from search groups. Since SearchGroup
			// is an ObservableCollection, this will make the search item disappear
			// from the ListView immediately. 
			_searchGroups[0].Remove(search);

			// But we should also update the backend. So, we use SearchService for that.
			_searchService.DeleteSearch(search.Id);
		}

		private void OnRefreshing(object sender, System.EventArgs e)
		{
			// Note that we're using the text in the SearchBar while refreshing
			// the list of searches. This ensures that the filter is applied 
			// while refreshing the list. 
			PopulateListView(_searchService.GetRecentSearches(searchBar.Text));

			listView.EndRefresh();
		}

		private void OnSearchSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			var search = e.SelectedItem as Search;
			DisplayAlert("Selected", search.Location, "OK");
		}
	}
}

