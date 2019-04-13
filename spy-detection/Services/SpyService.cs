using Microsoft.EntityFrameworkCore;
using spy_detection.Api;
using spy_detection.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace spy_detection.Services
{
    public class SpyService
    {
        private readonly ISpyRepository _repository;
        private readonly SpyDetector _spyDetector;

        public SpyService(ISpyRepository repository, SpyDetector spyDetector)
        {
            _repository = repository;
            _spyDetector = spyDetector;
        }

        public Task<List<Spy>> GetAllAsync()
        {
            return _repository.ToListAsync();
        }

        public async Task<Spy> CreateAsync(ApiModel.SpyModel spy)
        {
            if (null == spy?.Code || !spy.Code.Any() || string.IsNullOrWhiteSpace(spy.Name))
            {
                throw new ArgumentNullException("spy is missing or invlid.");
            }

            var spies = await _repository.ToListAsync();
            var existingSpy = spies.FirstOrDefault(sp => sp.Name == spy.Name);
            if (null != existingSpy)
            {
                throw new ArgumentException($"A spy with name {spy.Name} already exists.");
            }

            existingSpy = spies.FirstOrDefault(sp => sp.IsCodeEqual(spy.Code));
            if (null != existingSpy)
            {
                throw new ArgumentException($"Another spy with code [{string.Join(',', spy.Code)}] already exists.");
            }

            var newSpy = new Spy
            {
                Code = spy.Code,
                Name = spy.Name
            };
            newSpy = await _repository.CreateAsync(newSpy);

            return newSpy;
        }

        public async Task<Spy> UpdateAsync(int id, Spy spy)
        {
            if (spy == null)
                throw new ArgumentNullException("Spy to update is empty or missing.");
            if (null == spy?.Code || !spy.Code.Any() || string.IsNullOrWhiteSpace(spy.Name))
            {
                throw new ArgumentException("spy is missing or invlid.");
            }

            var existing = await _repository.GetByIdAsync(id);
            if (existing.Id != spy.Id)
                throw new UnauthorizedAccessException("User has not been granted access to update spy for other.");

            if (spy.Name != existing.Name)
            {
                var duplicate = await _repository.GetAllAsQueryable().FirstOrDefaultAsync(sp => sp.Name == spy.Name);
                if (null != duplicate)
                {
                    throw new ArgumentException($"Another spy with same name {spy.Name} already exists.");
                }

                existing.Name = spy.Name;
            }

            if (!spy.IsCodeEqual(existing.Code))
            {
                var spies = await _repository.ToListAsync();
                if (null != spies.FirstOrDefault(s => s.IsCodeEqual(spy.Code)))
                {
                    throw new ArgumentException($"Another spy with same code [{string.Join(',', spy.Code)}] already exists.");
                }

                existing.Code = spy.Code;
            }

            var updatedSpy = await _repository.UpdateAsync(existing);

            return updatedSpy;
        }

        public async Task<Spy> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                return null;
            }

            await _repository.DeleteAsync(existing);

            return existing;
        }

        public async Task<bool> DetectSpyAsync(int[] message, string name)
        {
            if (null == message || !message.Any())
            {
                throw new ArgumentException("message is missing or invlid");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name is missing or invlid");
            }

            var spy = await _repository.GetAllAsQueryable().FirstOrDefaultAsync(sp => sp.Name == name);
            if (null == spy)
            {
                throw new ArgumentException($"spy {name} is not found");
            }

            var contains = _spyDetector.ContainsSpy(message, spy.Code);
            return contains;
        }
    }
}
