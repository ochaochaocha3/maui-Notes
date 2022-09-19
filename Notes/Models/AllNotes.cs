using System.Collections.ObjectModel;

namespace Notes.Models;

internal class AllNotes
{
    public ObservableCollection<Note> Notes { get; set; } = new ObservableCollection<Note>();

    public AllNotes() => LoadNotes();

    public void LoadNotes()
    {
        Notes.Clear();

        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        var notes = Directory
            .EnumerateFiles(appDataPath, "*.notes.txt")
            .Select(filename => new Note()
            {
                Filename = filename,
                Text = File.ReadAllText(filename),
                Date = File.GetCreationTime(filename),
            })
            .OrderBy(note => note.Date);

        foreach (var n in notes)
        {
            Notes.Add(n);
        }
    }
}
