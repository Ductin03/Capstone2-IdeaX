﻿namespace IdeaX.Model.RequestModels
{
    public class CreateCategoryRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
