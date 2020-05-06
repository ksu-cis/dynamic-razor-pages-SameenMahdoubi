using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Movies.Pages
{
    public class IndexModel : PageModel
    {

        /// <summary>
        /// The movies to display on the index page 
        /// </summary>
        public IEnumerable<Movie> Movies { get; protected set; }

        /// <summary>
        /// The current search terms 
        /// </summary>
        public string SearchTerms { get; set; } = "";

        /// <summary>
        /// The filtered MPAA Ratings
        /// </summary>       
        public string[] MPAARating { get; set; }

        /// <summary>
        /// The filtered genres
        /// </summary>
        public string[] Genres { get; set; }

        /// <summary>
        /// The minimum IMDB Rating
        /// </summary>
        [BindProperty]
        public double? IMDBMin { get; set; }

        /// <summary>
        /// The maximum IMDB Rating
        /// </summary>
        [BindProperty]
        public double?  IMDBMax { get; set; }

        /// <summary>
        /// The minimum Rotten Tomatoes Rating
        /// </summary>
        [BindProperty]
        public int? RottenTomatoesMin { get; set; }

        /// <summary>
        /// The maximum Rotten Tomatoes Rating
        /// </summary>
        [BindProperty]
        public int? RottenTomatoesMax { get; set; }


        /// <summary>
        /// Does the response initialization for incoming GET requests
        /// </summary>
        public void OnGet(double? IMDBMin, double? IMDBMax, int? RottenTomatoesMax, int? RottenTomatoesMin)
        {
            // Nullable conversion workaround
            this.IMDBMin = IMDBMin;
            this.IMDBMax = IMDBMax;
            this.RottenTomatoesMax = RottenTomatoesMax;
            this.RottenTomatoesMin = RottenTomatoesMin;
            SearchTerms = Request.Query["SearchTerms"];
            MPAARating = Request.Query["MPAARating"];
            Genres = Request.Query["Genres"];

            Movies = MovieDatabase.All;
            // Search movie titles for the SearchTerms
            if (SearchTerms != null)
            {
                Movies = Movies.Where(movie => movie.Title != null && movie.Title.Contains(SearchTerms, StringComparison.InvariantCultureIgnoreCase));
            }

            if (MPAARating != null && MPAARating.Length != 0)
            {
                Movies = Movies.Where(movie =>
                    movie.MPAARating != null &&
                    MPAARating.Contains(movie.MPAARating)
                    );
            }

            if (Genres != null && Genres.Length != 0)
            {
                Movies = Movies.Where(movie =>
                    movie.MajorGenre != null &&
                    Genres.Contains(movie.MajorGenre)
                    );
            }

            // Nullcheck maximum first and filter by it, then do the same for minimum
            if(IMDBMax != null)
            {
                Movies = Movies.Where(movie => movie.IMDBRating != null && movie.IMDBRating <= IMDBMax);
            }

            if (IMDBMin != null)
            {
                Movies = Movies.Where(movie => movie.IMDBRating != null && movie.IMDBRating >= IMDBMin);
            }

            // And the same for Rotten Tomatoes rankings
            if (RottenTomatoesMax != null)
            {
                Movies = Movies.Where(movie => movie.RottenTomatoesRating != null && movie.RottenTomatoesRating <= RottenTomatoesMax);
            }

            if (RottenTomatoesMin != null)
            {
                Movies = Movies.Where(movie => movie.RottenTomatoesRating != null && movie.RottenTomatoesRating >= RottenTomatoesMin);
            }

            /* Old searching methods
            Movies = MovieDatabase.Search(SearchTerms);
            Movies = MovieDatabase.FilterByMPAARating(Movies, MPAARating);
            Movies = MovieDatabase.FilterByGenre(Movies, Genres);
            Movies = MovieDatabase.FilterByIMDBRating(Movies, IMDBMin, IMDBMax);
            Movies = MovieDatabase.FilterByRottenTomatoesRating(Movies, RottenTomatoesMin, RottenTomatoesMax);
            */

        }


    }
}
