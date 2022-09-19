using System.Diagnostics;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Notes.Views;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class NotePage : ContentPage
{
	public string ItemId
	{
		set
		{
			LoadNote(value);
		}
	}

	public NotePage()
	{
		InitializeComponent();

		string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		string randomFileName = $"{Path.GetRandomFileName()}.notes.txt";
		string path = Path.Combine(appDataPath, randomFileName);

		LoadNote(path);
	}

	/// <summary>
	/// 保存ボタンがクリックされたときの処理。
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private async void SaveButton_Clicked(object sender, EventArgs e)
	{
		if (BindingContext is Models.Note note)
		{
			// ファイルに保存する
			File.WriteAllText(note.Filename, TextEditor.Text);

            var toast = Toast.Make("Saved the message.");
			await toast.Show();
        }

        await Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// 削除ボタンがクリックされたときの処理。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void DeleteButton_Clicked(object sender, EventArgs e)
	{
		if (BindingContext is Models.Note note)
		{
			bool accepted = await DisplayAlert("Delete", "Are you sure to delete the message?", "Yes", "No");
			if (accepted)
			{
                if (File.Exists(note.Filename))
                {
                    // ファイルが存在したら削除する
                    File.Delete(note.Filename);

                    var toast = Toast.Make("Deleted the message.");
                    await toast.Show();
                }
			}
        }

		await Shell.Current.GoToAsync("..");
	}

	private void LoadNote(string fileName)
	{
		var noteModel = new Models.Note()
		{
			Filename = fileName
		};

		if (File.Exists(fileName))
		{
			noteModel.Date = File.GetCreationTime(fileName);
			noteModel.Text = File.ReadAllText(fileName);
		}

		BindingContext = noteModel;
	}
}