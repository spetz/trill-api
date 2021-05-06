using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Trill.Core.App.Commands
{
    public class SendStory
    {
        public Guid Id { get; }
        
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; }
        
        [Required]
        [StringLength(1000, MinimumLength = 5)]
        public string Text { get; }
        
        [Required]
        public string Author { get; }
        
        public IEnumerable<string> Tags { get; }

        public SendStory(Guid id, string title, string text, string author, IEnumerable<string> tags = null)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Title = title;
            Text = text;
            Author = author;
            Tags = tags ?? Enumerable.Empty<string>();
        }
    }
}