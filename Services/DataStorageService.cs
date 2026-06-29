using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using NexPresenter.Models;

namespace NexPresenter.Services;

public class DataStorageService
{
    private readonly string _dataDirectory;
    private readonly string _presentationsFile;

    public DataStorageService()
    {
        // Use cross-platform user data directory
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _dataDirectory = Path.Combine(appData, "NexPresenter");
        _presentationsFile = Path.Combine(_dataDirectory, "presentations.json");

        // Ensure directory exists
        Directory.CreateDirectory(_dataDirectory);
    }

    public List<Presentation> LoadPresentations()
    {
        if (!File.Exists(_presentationsFile))
        {
            return new List<Presentation>();
        }

        try
        {
            var json = File.ReadAllText(_presentationsFile);
            return JsonSerializer.Deserialize<List<Presentation>>(json) ?? new List<Presentation>();
        }
        catch
        {
            return new List<Presentation>();
        }
    }

    public void SavePresentations(List<Presentation> presentations)
    {
        var json = JsonSerializer.Serialize(presentations, new JsonSerializerOptions 
        { 
            WriteIndented = true 
        });
        File.WriteAllText(_presentationsFile, json);
    }

    public Presentation CreatePresentation(string name, string description = "")
    {
        var presentations = LoadPresentations();
        var presentation = new Presentation
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };
        presentations.Add(presentation);
        SavePresentations(presentations);
        return presentation;
    }

    public void UpdatePresentation(Presentation presentation)
    {
        var presentations = LoadPresentations();
        var index = presentations.FindIndex(p => p.Id == presentation.Id);
        if (index >= 0)
        {
            presentation.ModifiedAt = DateTime.Now;
            presentations[index] = presentation;
            SavePresentations(presentations);
        }
    }

    public void DeletePresentation(Guid id)
    {
        var presentations = LoadPresentations();
        presentations.RemoveAll(p => p.Id == id);
        SavePresentations(presentations);
    }

    public Presentation? GetPresentation(Guid id)
    {
        var presentations = LoadPresentations();
        return presentations.FirstOrDefault(p => p.Id == id);
    }
}
