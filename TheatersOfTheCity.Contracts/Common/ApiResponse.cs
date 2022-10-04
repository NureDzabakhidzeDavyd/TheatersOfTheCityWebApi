using System.Net;
using System.Runtime.Serialization;

namespace TheatersOfTheCity.Contracts.Common;

[DataContract]
public class ApiResponse<T>
{
    [DataMember]
    public bool IsSuccess { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public string? ErrorMessage { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public T? Result { get; set; }
}