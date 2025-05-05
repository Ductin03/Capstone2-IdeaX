using IdeaX.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdeaX.Services
{
    public class MatchingService
    {
        private readonly IdeaXDbContext _context;
        public MatchingService(IdeaXDbContext context)
        {
            _context = context;
        }
        public List<(User Investor, int Score)> MatchIdeaWithInvestors(Idea idea, IEnumerable<User> investors)
        {
            if (idea == null || investors == null)
                throw new ArgumentNullException("Idea or investors cannot be null");

            var results = new List<(User Investor, int Score)>();

            foreach (var investor in investors)
            {
                int score = CalculateMatchScore(idea, investor);
                results.Add((Investor: investor, Score: score)); 
            }

            return results.OrderByDescending(r => r.Score).ToList();
        }

        private int CalculateMatchScore(Idea idea, User investor)
        {
            int score = 0;
            if (idea == null || investor == null)
                return 0;

            // Khớp ngành (30 điểm)
            var category = _context.Categories
                .Where(c => c.Id == idea.CategoryId)
                .Select(c => c.Name)
                .FirstOrDefault();
            var ideaIndustries = category != null
                 ? new[] { category.ToLower().Trim() }
                 : Enumerable.Empty<string>();
            var investorIndustries = investor.PreferredIndustries?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                ?.Select(i => i.Trim().ToLower()) ?? Enumerable.Empty<string>();
            if (ideaIndustries.Intersect(investorIndustries).Any())
                score += 30;

            // Khớp giai đoạn (20 điểm)
            var investorStages = investor.PreferredStages?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                ?.Select(s => s.Trim().ToLower()) ?? Enumerable.Empty<string>();
            if (investorStages.Contains(idea.Stage?.ToLower() ?? ""))
                score += 20;

            // Khớp khu vực (20 điểm)
            var investorRegions = investor.PreferredRegions?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                ?.Select(r => r.Trim().ToLower()) ?? Enumerable.Empty<string>();
            if (investorRegions.Contains(idea.Region?.ToLower() ?? ""))
                score += 20;

            // Khớp vốn (20 điểm)
            if (investor.FundingRangeMin > 0 && investor.FundingRangeMax > 0 &&
                idea.Price >= investor.FundingRangeMin && idea.Price <= investor.FundingRangeMax)
                score += 20;

            // Khớp từ khóa (10 điểm)
            var ideaWords = idea.Description?.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                ?? Enumerable.Empty<string>();
            var investorWords = investor.PreferredIndustries?.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                ?? Enumerable.Empty<string>();
            if (ideaWords.Intersect(investorWords).Any())
                score += 10;

            return score;
        }
    }
}