namespace Crystal_Eyes_Controller.Dtos.Feedback
{
	public class FeedbackViewDto
	{
		public int FeedbackId { get; set; }
		public string Name { get; set; } = null!;
		public string? Image { get; set; }
		public string? Content { get; set; }
		public int? Star { get; set; }
	}
}
