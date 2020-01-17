using System;
using System.Collections.Generic;
using System.Linq;
using ListExercise.Models;

namespace ListExercise.Services
{
	// User's searches can be stored either in a database on the device, or 
	// somewhere in the cloud. We use SearchService to work with the persistence
	// store for the app. So this class hides the complexity behind accessing
	// the recent searches. Whether they're stored locally or in the cloud, it 
	// doesn't matter from the perspective of the user interface.

	// Note nowhere in this class we're using an ObservableCollection, because 
	// this is a collection type that we often use in Xamarin or WPF apps. 
	// In theory, we should be able to re-use this SearchService in an ASP.NET MVC 
	// web app. We don't have ObservableCollection in web apps. So, this class
	// should not know anything about ObservableCollection. GetRecentSearches method simply returns an
	// IEnumerable. 
	public class SearchService 
	{
		// Currently I've hardcoded the list of searches here. But they could be
		// stored in a database or fetched via a remote service. 
		private List<Search> _searches = new List<Search>
		{
			new Search
			{
				Id = 1,
				Location = "West Hollywood, CA, United States",
				CheckIn = new DateTime(2016, 9, 1),
				CheckOut = new DateTime(2016, 11, 1)
			},
			new Search
			{
				Id = 2,
				Location = "Santa Monica, CA, United States",
				CheckIn = new DateTime(2016, 9, 1),
				CheckOut = new DateTime(2016, 11, 1)
			}
		};

		public IEnumerable<Search> GetRecentSearches(string filter = null)
		{
			if (String.IsNullOrWhiteSpace(filter))
				return _searches;

			// Note that I've used StringComparison.CurrentCultureIgnoreCase 
			// so searching is case-insensitive.
			return _searches.Where(s => s.Location.StartsWith(filter, StringComparison.CurrentCultureIgnoreCase));
		}

		public void DeleteSearch(int searchId)
		{
			_searches.Remove(_searches.Single(s => s.Id == searchId));
		}
	}
}
