﻿namespace QuizApp.DTOs;

public class CreateQuizDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsGlobal { get; set; } = false;
}
