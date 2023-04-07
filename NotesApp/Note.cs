using NotesApp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public partial class Note : ObservableObject
    {
        private DateTime _creationDateTime = DateTime.Now;
        private string _encodedText = string.Empty;
        private string _decodedText = string.Empty;
        private string _textPreview = string.Empty;

        public int Id { get; set; }

        public DateTime CreationDateTime
        {
            get => _creationDateTime;
            set => Set(ref _creationDateTime, value);
        }

        public string EncodedText
        {
            get => _encodedText;
            set
            {
                Set(ref _encodedText, value);
            }
        }

        [NotMapped]
        public string DecodedText
        {
            get => _decodedText;
            set
            {
                Set(ref _decodedText, value);
                if (_decodedText.Length > 50)
                {
                    _textPreview = _decodedText.Substring(0, 50);
                    _textPreview += "...";
                }
                else
                {
                    _textPreview = _decodedText.Substring(0, _decodedText.Length);
                }
            }
        }

        [NotMapped]
        public string TextPreview
        {
            get => _textPreview;
            set
            {
                Set(ref _textPreview, value);
            }
        }
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propName = "")
        {
            if (Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propName);
            return true;
        }
    }
}
