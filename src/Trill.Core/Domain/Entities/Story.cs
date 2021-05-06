using System;
using System.Collections.Generic;
using System.Linq;
using Trill.Core.Exceptions;

namespace Trill.Core.Domain.Entities
{
    public class Story
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Text { get; private set; }
        public string Author { get; private set; }
        public IEnumerable<string> Tags { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Story()
        {
        }

        public Story(Guid id, string title, string text, string author, IEnumerable<string> tags)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Missing title.");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Missing text.");
            }

            if (string.IsNullOrWhiteSpace(author))
            {
                throw new MissingAuthorException();
            }

            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Title = title.Trim();
            Text = text.Trim();
            Author = author;
            Tags = tags ?? Enumerable.Empty<string>();
            CreatedAt = DateTime.UtcNow;
        }
    }
}