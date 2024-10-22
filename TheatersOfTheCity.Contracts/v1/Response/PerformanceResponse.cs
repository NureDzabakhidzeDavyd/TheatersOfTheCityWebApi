﻿using TheatersOfTheCity.Contracts.v1.Response;

namespace TheatersOfTheCity.Contracts.v1.Request;

public class PerformanceResponse
{
    public int PerformanceId { get; set; }
    
    public string Name { get; set; }
    
    public string Genre { get; set; }
    
    public TimeSpan Duration { get; set; }
    
    public string Language { get; set; }
    
    public IEnumerable<ParticipantResponse> Participants { get; set; }
}