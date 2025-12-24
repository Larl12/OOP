# shop/management/commands/seed_demo.py
from typing import Optional, List
from django.core.management.base import BaseCommand
from django.core.files.base import ContentFile
from django.db import transaction
from django.conf import settings
import os

from ecom.models import Order, InvoiceFile, User

DEMO_USERS = [
    {"username":"admin","email":"admin@shop.local","is_staff":True,"is_superuser":True,"is_mgr":True,"password":"password"},
    {"username":"mgr_alice","email":"alice@shop.local","is_staff":True,"is_superuser":False,"is_mgr":True,"password":"password"},
    {"username":"user_bob","email":"bob@shop.local","is_staff":False,"is_superuser":False,"is_mgr":False,"password":"password"},
]

SAMPLE_INVOICE = b"Demo invoice for order %d\nUploaded by: %s\n"

class Command(BaseCommand):
    help = "Seed demo data for shop app"

    def handle(self, *args, **options):
        with transaction.atomic():
            self.stdout.write("Seeding shop demo...")
            users = self._create_users()
            
            managers = [u for u in users if getattr(u, "is_mgr", False)]
            
            created_orders = []
            created_invoices = []

            for i in range(1, 4):
                if managers:
                    owner = managers[i % len(managers)]
                elif users:
                    owner = users[0]
                else:
                    self.stdout.write(self.style.ERROR("Нет пользователей для создания заказов"))
                    break
                    
                o, _ = Order.objects.get_or_create(customer=owner, total=10.0 * i)
                created_orders.append(o)
                
                inv = InvoiceFile(order=o)
                fname = f"order_{o.id}_invoice.txt"
                inv.filename = fname
                
                content = SAMPLE_INVOICE % (o.id, owner.username)
                inv.file.save(fname, ContentFile(content), save=True)
                created_invoices.append(inv)
                self.stdout.write(f"  + order #{o.id} invoice -> {inv.file.name}")

            self._create_static_backup()

            self.stdout.write(self.style.SUCCESS("Users: " + ", ".join(u.username for u in users)))
            self.stdout.write("Orders: " + ", ".join(str(o.id) for o in created_orders))
            self.stdout.write(self.style.SUCCESS("Shop demo seeded."))

    def _create_users(self) -> List[User]:
        out = []
        for cfg in DEMO_USERS:
            u, created = User.objects.get_or_create(
                username=cfg["username"], 
                defaults={"email": cfg["email"]}
            )
            
            changed = False
            if created:
                u.set_password(cfg["password"])
                changed = True
                
            for f in ("is_staff", "is_superuser"):
                if getattr(u, f) != cfg[f]:
                    setattr(u, f, cfg[f])
                    changed = True
            
            try:
                if hasattr(u, "is_mgr") and getattr(u, "is_mgr") != cfg.get("is_mgr", False):
                    setattr(u, "is_mgr", cfg.get("is_mgr", False))
                    changed = True
            except (AttributeError, ValueError) as e:
                self.stdout.write(self.style.WARNING(f"Не удалось установить is_mgr для {u.username}: {e}"))
            
            if changed:
                u.save()
                self.stdout.write(self.style.SUCCESS(f"  + user {u.username}, password: {cfg['password']}"))
            else:
                self.stdout.write(f"  = user {u.username} (unchanged)")
            out.append(u)
        return out
    def _create_static_backup(self):
        static_dirs = getattr(settings, "STATICFILES_DIRS", None)
        
        if static_dirs:
            if isinstance(static_dirs, (list, tuple)) and len(static_dirs) > 0:
                target = static_dirs[0]
            else:
                target = static_dirs
        else:
            target = os.path.join(settings.BASE_DIR, "static")
        
        backup_dir = os.path.join(target, "backups")
        os.makedirs(backup_dir, exist_ok=True)
        
        backup_file = os.path.join(backup_dir, ".env.backup")
        try:
            with open(backup_file, "wb") as f:
                f.write(b"SHOP_FAKE_SECRET=demo")
            self.stdout.write(f"  + created {backup_file}")
        except (IOError, PermissionError) as e:
            self.stdout.write(self.style.ERROR(f"Не удалось создать backup файл: {e}"))