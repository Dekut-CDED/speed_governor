using System;
namespace Api.Background
{
    public class EventMessage
    {
        public string Id { get; }
        public string Title {get; }
        public DateTime CreateDateTime { get; set; }

        public EventMessage(string id, string title, DateTime createdDatetime)
        {
            Id = id;
            Title = title;
            CreateDateTime = createdDatetime;
        }
    }
}