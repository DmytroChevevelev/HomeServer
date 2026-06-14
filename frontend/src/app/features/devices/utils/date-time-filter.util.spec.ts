import { validateDateTimeRange } from './date-time-filter.util';

describe('validateDateTimeRange', () => {
  it('returns valid state when range is correct', () => {
    const result = validateDateTimeRange({
      fromUtc: '2026-01-01T00:00:00.000Z',
      toUtc: '2026-01-02T00:00:00.000Z'
    });

    expect(result.isValidRange).toBeTrue();
    expect(result.validationMessage).toBe('');
  });

  it('returns invalid state when fromUtc is later than toUtc', () => {
    const result = validateDateTimeRange({
      fromUtc: '2026-01-03T00:00:00.000Z',
      toUtc: '2026-01-02T00:00:00.000Z'
    });

    expect(result.isValidRange).toBeFalse();
    expect(result.validationMessage).toContain('earlier than or equal');
  });

  it('returns invalid state when date format is invalid', () => {
    const result = validateDateTimeRange({
      fromUtc: 'not-a-date',
      toUtc: '2026-01-02T00:00:00.000Z'
    });

    expect(result.isValidRange).toBeFalse();
    expect(result.validationMessage).toContain('must be valid');
  });
});
