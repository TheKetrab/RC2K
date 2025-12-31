using RC2K.DomainModel;

namespace RC2K.Presentation.Blazor.Views.Components;

public class TimeEntryListItem
{
    private TimeEntryList list;
    public TimeEntryListItem(TimeEntryList list, TimeEntry data)
    {
        Data = data;
        this.list = list;
    }

    public int PlaceByCar { get; set; }
    public int Place { get; set; }
    public TimeEntry Data { get; set; }

    public bool Verified => this.Data.VerifyInfo is not null;
    public string TimeDisplay => this.Data.Time.ToString("m:ss.ff");
    public TimeSpan Gap => this.Data.Time - list.best.Time;
    public string GapDisplay => Gap == TimeSpan.Zero ? "" : "-" + Gap.ToString("m\\:ss\\.ff");
    public string UploadTimeDisplay => this.Data.UploadTime == new DateTime(1,1,1) ? "" : this.Data.UploadTime.ToString("yyyy/MM/dd");
    public IEnumerable<string> Labels => (this.Data.Labels ?? "").Split(",").Where(x => !string.IsNullOrEmpty(x));

    public string DisplayName => Data.Driver.Known ? Data.Driver.User!.Name : Data.Driver.Name;

    public string PointsDisplay => string.Concat(
        (new int[] { GeneralPoints, CarPoints })
            .Where(x => x > 0)
            .Select(x => $"+{x}")
    );

    public int Points => GeneralPoints + CarPoints;

    private int _gp = -1;
    public int GeneralPoints
    {
        get
        {
            if (_gp == -1)
            {
                _gp = (list.generalPoints.TryGetValue(Data.Id, out int gp) ? gp : 0);
            }
            return _gp;
        }
        set
        {
            _gp = value;
        }
    }
    private int _cp = -1;
    public int CarPoints
    {
        get
        {
            if (_cp == -1)
            {
                _cp = (list.carPoints.TryGetValue(Data.Id, out int cp) ? cp : 0);
            }
            return _cp;
        }
        set
        {
            _cp = value;
        }
    }

}


