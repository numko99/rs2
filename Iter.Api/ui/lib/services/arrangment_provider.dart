import 'package:ui/models/arrangment.dart';
import 'base_provider.dart';

class ArrangmentProvider extends BaseProvider<Arrangement> {
  ArrangmentProvider() : super("Arrangement");

  @override
  Arrangement fromJson(data) {
    return Arrangement.fromJson(data);
  }
}