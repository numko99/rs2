import '../models/agency.dart';
import 'base_provider.dart';

class AgencyProvider extends BaseProvider<Agency> {
  AgencyProvider() : super("Agency");

  @override
  Agency fromJson(data) {
    return Agency.fromJson(data);
  }
}