using MAContracts.Contracts.Mappers.AI;
using MADTOs.DTOs.EntityFrameworkDTOs.AI;
using MAModels.EntityFrameworkModels.AI;

namespace MAServices.Mappers.AI
{
    public class SessionDtoObjectsMapper : ISessionDtoObjectsMapper
    {
        public SessionDtoObjectsMapper() { }

        public SessionsDTO SessionMappingDto(Sessions session, List<Requests> listRequests, List<Recommendations> listRecommendations)
        {
            SessionsDTO sessionDTO = new SessionsDTO();
            sessionDTO.SessionId = session.SessionId;            
            sessionDTO.DateTimeCreation = session.DateTimeCreation;
            List<RequestsDTO> requestsDTOs = new List<RequestsDTO>();
            foreach (Requests request in listRequests)
            {
                RequestsDTO requestDTO = new RequestsDTO();
                requestDTO.RequestId = request.RequestId;
                requestDTO.WhatClientWants = request.WhatClientWants;
                requestDTO.HowClientFeels = request.HowClientFeels;
                requestDTO.Sentiment = request.Sentiment != null ? (bool)request.Sentiment : null;
                var listRecomsFilterId = new List<Recommendations>();
                foreach(var recomm in listRecommendations)
                {
                    if(recomm.RequestId == requestDTO.RequestId) 
                        listRecomsFilterId.Add(recomm);
                }
                List<RecommendationsDTO> recommsDtos = new List<RecommendationsDTO>();
                listRecomsFilterId.ForEach(recomFilter =>
                {
                    RecommendationsDTO recomDTO = new RecommendationsDTO
                    {
                        RecommendationId = recomFilter.RecommendationId,
                        MovieId = recomFilter.MovieId,
                        MovieTitle = recomFilter.MovieTitle,
                        Name = recomFilter.Name,
                        LastName = recomFilter.LastName,
                        Email = recomFilter.Email,
                        AiScore = recomFilter.AiScore,
                        See = recomFilter.See
                    };
                });
                requestDTO.Recommendations = recommsDtos;
                requestsDTOs.Add(requestDTO);
            }
            sessionDTO.Requests = requestsDTOs;
            return sessionDTO;
        }
    }
}
