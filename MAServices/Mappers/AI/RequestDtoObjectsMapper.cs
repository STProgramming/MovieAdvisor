using MAContracts.Contracts.Mappers.AI;
using MADTOs.DTOs.EntityFrameworkDTOs.AI;
using MADTOs.DTOs.EntityFrameworkDTOs.Identity;
using MAModels.EntityFrameworkModels.AI;

namespace MAServices.Mappers.AI
{
    public class RequestDtoObjectsMapper : IRequestDtoObjectsMapper
    {
        public RequestDtoObjectsMapper() { }

        public RequestsDTO RequestMappingDto(Requests request, List<Recommendations> recoms)
        {            
            RequestsDTO requestDTO = new RequestsDTO();
            requestDTO.RequestId = request.RequestId;
            requestDTO.WhatClientWants = request.WhatClientWants;
            requestDTO.HowClientFeels = request.HowClientFeels;
            requestDTO.Sentiment = requestDTO.Sentiment;
            requestDTO.DateTimeRequest = request.DateTimeRequest;

            List<RecommendationsDTO> recomDtos = new List<RecommendationsDTO>();
            foreach (var recom in recoms)
            {
                RecommendationsDTO recomDto = new RecommendationsDTO();
                recomDto.RecommendationId = recom.RecommendationId;
                recomDto.MovieId = recom.MovieId;
                recomDto.Name = recom.Name;
                recomDto.LastName = recom.LastName;
                recomDto.Email = recom.Email;
                recomDto.AiScore = recom.AiScore;
                recomDto.Request = requestDTO;
                recomDtos.Add(recomDto);
            }

            requestDTO.Recommendations = recomDtos;
            return requestDTO;
        }
    }
}
