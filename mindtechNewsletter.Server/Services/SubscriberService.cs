using AutoMapper;
using mindtechNewsletter.Server.DTOs;
using mindtechNewsletter.Server.Models;
using mindtechNewsletter.Server.Repositories;
using mindtechNewsletter.Server.Responses;

namespace mindtechNewsletter.Server.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly ISubscriberRepository _repo;
        private readonly IMapper _mapper;

        public SubscriberService(ISubscriberRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<(ResponseModel<SubscriberReadDTO> Response, bool Created)> SubscribeAsync(SubscriberCreateDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email))
            {
                return (ResponseModel<SubscriberReadDTO>.Fail("Email inválido."), false);
            }

            var email = dto.Email.Trim().ToLowerInvariant();
            var existing = await _repo.GetByEmailAsync(email);

            if (existing != null && existing.IsActive)
            {
                return (ResponseModel<SubscriberReadDTO>.Fail("Email já inscrito."), false);
            }

            if (existing != null && !existing.IsActive)
            {
                existing.IsActive = true;
                _repo.Update(existing);
                await _repo.SaveChangesAsync();

                var reactivatedDto = _mapper.Map<SubscriberReadDTO>(existing);
                return (ResponseModel<SubscriberReadDTO>.Ok(reactivatedDto, "Inscrição reativada."), false);
            }

            var model = _mapper.Map<Subscriber>(dto);
            model.Email = email;
            model.IsActive = true;

            await _repo.AddAsync(model);
            await _repo.SaveChangesAsync();

            var createdDto = _mapper.Map<SubscriberReadDTO>(model);
            return (ResponseModel<SubscriberReadDTO>.Ok(createdDto, "Inscrito com sucesso."), true);
        }

        public async Task<(ResponseModel<object> Response, bool NotFound)> UnsubscribeAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return (ResponseModel<object>.Fail("Email inválido."), false);
            }

            var normalized = email.Trim().ToLowerInvariant();
            var existing = await _repo.GetByEmailAsync(normalized);

            if (existing == null || !existing.IsActive)
            {
                return (ResponseModel<object>.Fail("Email não encontrado ou já descadastrado."), true);
            }

            existing.IsActive = false;
            _repo.Update(existing);
            await _repo.SaveChangesAsync();

            return (ResponseModel<object>.Ok(null, "Descadastrado com sucesso."), false);
        }
    }
}
