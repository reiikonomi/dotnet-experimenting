global using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly DataContext _context;

        private readonly IMapper _mapper;
        public CharacterService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterdto newCharacter)
        {
            var characters = await _context.Characters.ToListAsync();
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);
            await _context.Characters.AddAsync(character);
            serviceResponse.Data = characters.Select(x => _mapper.Map<GetCharacterDto>(x)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.ToListAsync();
            serviceResponse.Data = dbCharacters.Select(x => _mapper.Map<GetCharacterDto>(x)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var characters = await _context.Characters.ToListAsync();
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = characters.FirstOrDefault(x => x.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterdto updatedCharacter)
        {

            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var characters = await _context.Characters.ToListAsync();
                var character = characters.FirstOrDefault(x => x.Id == updatedCharacter.Id);
                if (character is null)
                    throw new Exception($"Character with Id `{updatedCharacter.Id}` not found.");

                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Class = updatedCharacter.Class;
                character.Defense = updatedCharacter.Defense;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Strength = updatedCharacter.Strength;
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }


            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacterWithId(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var characters = await _context.Characters.ToListAsync();
            try
            {
                var character = characters.First(x => x.Id == id);
                if (character is null)
                    throw new Exception($"Character with Id `{id}` not found.");

                characters.Remove(character);
                serviceResponse.Data = characters.Select(x => _mapper.Map<GetCharacterDto>(x)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }


            return serviceResponse;
        }
    }
}