import { DevicePagesUiState, UiSurfaceState } from '../models/device-pages.models';

export function mapCollectionState<T>(
  items: T[] | null,
  isLoading: boolean,
  errorMessage: string | null = null
): UiSurfaceState {
  if (isLoading) {
    return { status: 'loading', errorMessage: null };
  }

  if (errorMessage) {
    return { status: 'error', errorMessage };
  }

  if (!items || items.length === 0) {
    return { status: 'empty', errorMessage: null };
  }

  return { status: 'ready', errorMessage: null };
}

export function mapDetailState<T>(
  item: T | null,
  isLoading: boolean,
  errorMessage: string | null = null
): UiSurfaceState {
  if (isLoading) {
    return { status: 'loading', errorMessage: null };
  }

  if (errorMessage) {
    return { status: 'error', errorMessage };
  }

  return {
    status: item ? ('ready' satisfies DevicePagesUiState) : ('empty' satisfies DevicePagesUiState),
    errorMessage: null
  };
}
