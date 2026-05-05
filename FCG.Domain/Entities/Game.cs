using FCG.Domain.Enums;

namespace FCG.Domain.Entities
{
    public class Game
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public GameGenre Genre { get; private set; }
        public string Developer { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsActive { get; private set; }

        protected Game() { }

        public Game(string title, string description, decimal price, GameGenre genre, string developer, DateTime releaseDate)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Price = price;
            Genre = genre;
            Developer = developer;
            ReleaseDate = releaseDate;
            IsActive = true;
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be empty.");

            Title = title.Trim();
            Touch();
        }

        public void SetDescription(string description) {
            Description = description.Trim();
            Touch();
        }

        public void SetPrice(decimal price)
        {
            if (price < 0) throw new ArgumentException("Price cannot be negative.");

            Price = price;
            Touch();
        }

        public void SetGenre(GameGenre genre)
        {
            Genre = genre;
            Touch();
        }

        public void SetDeveloper(string developer)
        {
            if (string.IsNullOrWhiteSpace(developer)) throw new ArgumentException("Developer cannot be empty.");

            Developer = developer.Trim();
            Touch();
        }

        public void Deactivate()
        {
            IsActive = false;
            Touch();
        }

        public void Activate()
        {
            IsActive = true;
            Touch();
        }

        private void Touch()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
