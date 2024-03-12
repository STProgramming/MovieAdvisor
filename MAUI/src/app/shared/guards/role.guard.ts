import { CanActivateFn } from '@angular/router';
import { RoleService } from '../services/role.service';
import { ɵɵinject } from '@angular/core';

export const roleGuard: CanActivateFn = (route, state) => {
  var roleService = ɵɵinject(RoleService);
  return roleService.isAppAdmin(true);
};
