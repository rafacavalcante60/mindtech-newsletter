using AutoMapper;
using mindtechNewsletter.Server.DTOs;
using mindtechNewsletter.Server.Models;
using mindtechNewsletter.Server.Repositories;
using mindtechNewsletter.Server.Responses;

namespace mindtechNewsletter.Server.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly ISubscriberRepository _repository;
        private readonly IMapper _mapper;

        public SubscriberService(ISubscriberRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseModel<SubscriberReadDTO>> SubscribeAsync(SubscriberCreateDTO dto)
        {
            var existing = await _repository.GetByEmailAsync(dto.Email);

            if (existing != null)
            {
                if (existing.IsActive)
                {
                    return ResponseModel<SubscriberReadDTO>.Fail("O email já está cadastrado.");
                }

                existing.IsActive = true;
                _repository.Update(existing);
                await _repository.SaveChangesAsync();

                var readDtoReactivated = _mapper.Map<SubscriberReadDTO>(existing);
                return ResponseModel<SubscriberReadDTO>.Ok(readDtoReactivated, "Inscrição reativada com sucesso.");
            }

            var subscriber = _mapper.Map<Subscriber>(dto);
            await _repository.AddAsync(subscriber);
            await _repository.SaveChangesAsync();

            var readDto = _mapper.Map<SubscriberReadDTO>(subscriber);
            return ResponseModel<SubscriberReadDTO>.Ok(readDto, "Inscrição realizada com sucesso.");
        }

        public async Task<ResponseModel<SubscriberReadDTO>> UnsubscribeAsync(SubscriberCreateDTO dto)
        {
            var existing = await _repository.GetByEmailAsync(dto.Email);

            if (existing == null || !existing.IsActive)
            {
                return ResponseModel<SubscriberReadDTO>.Fail("Email não encontrado ou já descadastrado.");
            }

            existing.IsActive = false;
            _repository.Update(existing);
            await _repository.SaveChangesAsync();

            var readDto = _mapper.Map<SubscriberReadDTO>(existing);
            return ResponseModel<SubscriberReadDTO>.Ok(readDto, "Descadastro realizado com sucesso.");
        }
    }
}
