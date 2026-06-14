import { DeviceHistoryFilterState } from '../models/device-pages.models';

export interface DateTimeRangeInput {
  fromUtc: string;
  toUtc: string;
}

function parseDate(value: string): Date | null {
  const parsed = new Date(value);
  if (Number.isNaN(parsed.getTime())) {
    return null;
  }

  return parsed;
}

export function validateDateTimeRange(input: DateTimeRangeInput): DeviceHistoryFilterState {
  const fromDate = parseDate(input.fromUtc);
  const toDate = parseDate(input.toUtc);

  if (!fromDate || !toDate) {
    return {
      fromUtc: input.fromUtc,
      toUtc: input.toUtc,
      isValidRange: false,
      validationMessage: 'Both date-time values must be valid.'
    };
  }

  if (fromDate.getTime() > toDate.getTime()) {
    return {
      fromUtc: input.fromUtc,
      toUtc: input.toUtc,
      isValidRange: false,
      validationMessage: 'From date-time must be earlier than or equal to To date-time.'
    };
  }

  return {
    fromUtc: input.fromUtc,
    toUtc: input.toUtc,
    isValidRange: true,
    validationMessage: ''
  };
}
