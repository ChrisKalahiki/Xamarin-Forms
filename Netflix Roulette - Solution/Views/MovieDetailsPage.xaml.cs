using System;
using Xamarin.Forms;

namespace MovieApp
{
	public partial class MovieDetailsPage : ContentPage
	{
		private MovieService _movieService = new MovieService();
		private Movie _movie;

		public MovieDetailsPage(Movie movie)
		{
			if (movie == null)
				throw new ArgumentNullException(nameof(movie));

			_movie = movie;

			InitializeComponent();
		}

		protected override async void OnAppearing()
		{
			BindingContext = await _movieService.GetMovie(_movie.Title);

			base.OnAppearing();
		}
	}
}
